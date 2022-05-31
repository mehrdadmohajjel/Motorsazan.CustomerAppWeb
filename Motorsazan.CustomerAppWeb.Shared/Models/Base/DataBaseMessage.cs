using System;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Base
{
    public class DataBaseMessage
    {
        public string Category { set; get; }

        public string ErrorCode { set; get; }

        public string LongDescription { set; get; }

        public string BriefDescription { set; get; }

        public bool GetErrorBriefDescription()
        {
            var result = GetErrorBriefDescription(Category, ErrorCode);
            if (result.Status != 1 || result.Code != 1) return false;

            BriefDescription = (string) result.Value;
            return true;
        }

        public static WebServiceResult GetErrorBriefDescription(string category, string errorCode)
        {
            if (category is null) throw new ArgumentNullException(nameof(category));

            if (errorCode is null) throw new ArgumentNullException(nameof(errorCode));

            return new WebServiceResult {Status = 1, Code = 1, Text = "General Message", Value = "General Message"};
        }
    }
}