namespace QuickCode.Turuncu.Gateway.Models;

public class KafkaMessage
{
    public RequestInfo RequestInfo { get; set; }
    public ResponseInfo ResponseInfo { get; set; }
    public string ExceptionMessage { get; set; }
    public int ElapsedMiliseconds { get; set; }
    public DateTime Timestamp { get; set; }
}