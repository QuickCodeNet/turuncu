namespace QuickCode.Turuncu.Gateway.Models;

public class RequestInfo
{
    public string Path { get; set; }
    public string Method { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    
    public string Body { get; set; }
}