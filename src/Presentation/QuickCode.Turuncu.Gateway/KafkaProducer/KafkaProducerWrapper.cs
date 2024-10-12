using Confluent.Kafka;

namespace QuickCode.Turuncu.Gateway.KafkaProducer;

public class KafkaProducerWrapper : IKafkaProducerWrapper, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerWrapper(IConfiguration config)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"],
            MessageSendMaxRetries = 3,
            RetryBackoffMs = 1000
        };
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
    }

    public async Task ProduceAsync(string topic, string key, string message)
    {
        await _producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = message });
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}