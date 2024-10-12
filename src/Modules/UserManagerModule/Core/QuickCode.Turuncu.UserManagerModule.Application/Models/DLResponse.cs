namespace QuickCode.Turuncu.UserManagerModule.Application.Models
{
    public class DLResponse<T>
    {
        public DLResponse() 
        {
            this.Message = "Success";
            this.Value = default(T)!;
            this.Code = 0;
        }
        public DLResponse(T value, string message)
        {
            this.Code = 0;
            this.Message = message;
            this.Value = value;
        }
        
        public DLResponse(T value)
        {
            this.Code = 0;
            this.Message = "Success";
            Value = value;
        }

        public string Message { get; set; }
        public int Code { get; set; }
        public T Value { get; set; } 
    }
}
