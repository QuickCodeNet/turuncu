using QuickCode.Turuncu.Common.Model;

namespace QuickCode.Turuncu.Common.Helpers
{
    public static class ErrorModelHelper
	{
        public static ErrorModel CreateErrorModel(string errorCode, params string[] detailErrorCodes)
        {
            var errorModel = new ErrorModel();
            errorModel.ErrorCode = errorCode;
            if (detailErrorCodes.Any())
            {
                errorModel.DetailErrorCodes = detailErrorCodes.ToList();
            }
            return errorModel;
        }
    }
}

