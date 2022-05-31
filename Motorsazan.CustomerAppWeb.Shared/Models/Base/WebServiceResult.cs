namespace Motorsazan.CustomerAppWeb.Shared.Models.Base
{
    public class WebServiceResult
    {
        public int Code { get; set; }

        public int Status { get; set; }

        public string Text { get; set; }

        public object Value { get; set; }
    }
}