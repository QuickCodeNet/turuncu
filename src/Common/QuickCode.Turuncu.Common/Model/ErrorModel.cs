namespace QuickCode.Turuncu.Common.Model
{
    public class ErrorModel
    {
        public string ErrorCode { get; set; } = default!;
        public List<string> DetailErrorCodes { get; set; } = new();
    }
}

