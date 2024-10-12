using System.ComponentModel;
using System.Text.Json.Serialization;

namespace QuickCode.Turuncu.Common.Model
{
    public class SubmitMessage
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public string? MessageText { get; set; }
        
        [DefaultValue("email/sms")] 
        public string Provider { get; set; } = default!;
        
        public string ProviderAddress { get; set; } = default!;
        
        public override string ToString()
        {
            return $"Id : {Id} Message : {MessageText} ProviderAddress : {ProviderAddress} CreatedDate : {CreatedDate}";
        }
    }
}
