using System.Data;
using Motorsazan.CustomerAppWeb.Shared.Attributes;
using Motorsazan.CustomerAppWeb.Shared.Models.DomainModels;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider
{
    public class InputAddImageToNotificationSlider
    {
        [StoredProcedureParameter(SqlDbType = SqlDbType.Structured)]
        public Tbl_FileDetail[] FileDetails { get; set; }

        [IgnoreInStoredProcedureParameters] public string[] Base64Values { get; set; }

        [IgnoreInStoredProcedureParameters] public string[] FileExtensions { get; set; }
    }
}