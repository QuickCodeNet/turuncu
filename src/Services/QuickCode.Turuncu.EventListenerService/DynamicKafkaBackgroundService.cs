using Confluent.Kafka;
using System.Collections.Concurrent;
using Confluent.Kafka.Admin;
using QuickCode.Turuncu.Common.Nswag;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;

namespace QuickCode.Turuncu.EventListenerService;

public class DynamicKafkaBackgroundService : BackgroundService
{
    private readonly ILogger<DynamicKafkaBackgroundService> _logger;
    private readonly ConsumerConfig _consumerConfig;
    private readonly IConfiguration _configuration;
    private readonly IKafkaEventsClient _kafkaEventsClient;
    private readonly ConcurrentBag<string> _topics = [];
    private readonly ConcurrentDictionary<string, (Task, CancellationTokenSource)> _consumers = new ConcurrentDictionary<string, (Task, CancellationTokenSource)>();
    private static int _topicRefreshInterval =  10 * 60;
    
    private static int _topicListenerInterval =  60;
    public static void SetTopicRefreshInterval(int seconds)
    {
        _topicRefreshInterval = seconds;
    }
    
    public static void SetTopicListenerInterval(int seconds)
    {
        _topicListenerInterval = seconds;
    }
    
    public static int GetTopicRefreshInterval()
    {
        return _topicRefreshInterval;
    }
    
    public static int GetTopicListenerInterval()
    {
        return _topicListenerInterval;
    }

    
    public DynamicKafkaBackgroundService(ILogger<DynamicKafkaBackgroundService> logger, IConfiguration configuration, IKafkaEventsClient kafkaEventsClient)
    {
        _logger = logger;
        _kafkaEventsClient = kafkaEventsClient;
        _configuration = configuration;
        
        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            GroupId = _configuration["Kafka:GroupId"],
            AutoOffsetReset = AutoOffsetReset.Latest,
        };
    }
    
    private void AddTopic(string newTopic)
    {
        if (!_topics.Contains(newTopic))
        {
            _topics.Add(newTopic);
            _logger.LogInformation($"New topic added: {newTopic}");
            
            var cancellationTokenSource = new CancellationTokenSource();
            var consumerTask = Task.Run(() => ListenToTopic(newTopic, cancellationTokenSource.Token), cancellationTokenSource.Token);
            _consumers.TryAdd(newTopic, (consumerTask, cancellationTokenSource));
        }
    }
    
    private void RemoveTopic(string topic)
    {
        if (_consumers.TryRemove(topic, out var consumerInfo))
        {
            _logger.LogInformation($"Topic removed: {topic}");
            consumerInfo.Item2.Cancel();
        }
        else
        {
            _logger.LogWarning($"Topic {topic} not found to remove.");
        }
    }

    private async Task<List<string>> GetAllTopicNames(CancellationToken stoppingToken)
    {
        var apiKeyConfigValue = $"QuickCodeApiKeys:UserManagerModuleApiKey";
        var configApiKey = _configuration.GetValue<string>(apiKeyConfigValue);
        (_kafkaEventsClient as ClientBase)!.SetApiKey(configApiKey!);
        var allEvents = await _kafkaEventsClient.GetKafkaEventsAsync(stoppingToken);
        var allTopicNames = new List<string>();
        var onCompleteEvents = allEvents.
            Where(i => i.OnComplete).
            Select(i => $"{i.TopicName}_{i.HttpMethod.ToLower()}__on_complete").ToList();

        var onErrorEvents = allEvents.
            Where(i => i.OnError).
            Select(i => $"{i.TopicName}_{i.HttpMethod.ToLower()}__on_error").ToList();
        
        var onTimeoutEvents = allEvents.
            Where(i => i.OnTimeout).
            Select(i => $"{i.TopicName}_{i.HttpMethod.ToLower()}__on_timeout").ToList();

        
        allTopicNames.AddRange(onCompleteEvents);
        allTopicNames.AddRange(onErrorEvents);
        allTopicNames.AddRange(onTimeoutEvents);
        return allTopicNames;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Background task running at: {time}", DateTimeOffset.Now);
                var activeTopicsList = await GetAllTopicNames(stoppingToken);
                var unusedTopicsList = _topics.Where(i => !activeTopicsList.Contains(i)).ToList();
                var newTopicList = activeTopicsList.Where(i => !_topics.Contains(i)).ToList();

                _logger.LogInformation(
                    "Active: {countActive} / Unused: {countActive} / New: {countActive} Topics",
                    activeTopicsList.Count, unusedTopicsList.Count, newTopicList.Count);
                
                foreach (var topicName in unusedTopicsList)
                {
                    RemoveTopic(topicName);
                }

                foreach (var topicName in newTopicList)
                {
                    AddTopic(topicName);
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_topicRefreshInterval), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Task was cancelled.");
                    break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DynamicKafkaBackgroundService Error: {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
    
    private async Task ListenToTopic(string topic, CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Message received from {topic}: {consumeResult.Message.Value}");
                }
                catch (ConsumeException ex)
                {
                    await CreateTopic(topic);
                    consumer.Subscribe(topic);
                    _logger.LogError($"Error consuming message from {topic}: {ex.Error.Reason}");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation($"Consumer for topic {topic} cancelled.");
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(_topicListenerInterval), stoppingToken);
            }
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task CreateTopic(string topicName)
    {
        var kafkaBootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers");
        var config = new AdminClientConfig
        {
            BootstrapServers = kafkaBootstrapServers
        };

        using var adminClient = new AdminClientBuilder(config).Build();
        try
        {
            var topicSpecifications = new TopicSpecification
            {
                Name = topicName
            };
            
            await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecifications });

            Console.WriteLine($"Topic '{topicName}' successfully created.");
        }
        catch (CreateTopicsException e)
        {
            if (e.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
            {
                Console.WriteLine($"Topic '{topicName}' already exists.");
            }
            else
            {
                Console.WriteLine($"An error occurred creating topic '{topicName}': {e.Results[0].Error.Reason}");
            }
        }
    }
}