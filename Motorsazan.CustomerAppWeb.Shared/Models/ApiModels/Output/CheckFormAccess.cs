namespace Motorsazan.CustomerAppWeb.Shared.Models.ApiModels.Output
{
    public class CheckFormAccess
    {
        public Status Status { get; set; }
        public CheckFormAccessParams Params { get; set; }

        public class CheckFormAccessParams
        {
            public FormAccessInfo HasAccessToPage { get; set; }
        }

        public class FormAccessInfo
        {
            public bool HasAccess { get; set; }
        }
    }
}