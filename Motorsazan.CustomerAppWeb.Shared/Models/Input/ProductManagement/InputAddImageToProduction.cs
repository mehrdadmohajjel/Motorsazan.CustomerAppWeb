using Motorsazan.CustomerAppWeb.Shared.Attributes;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement
{
    public class InputAddImageToProduction
    {
        [StoredProcedureParameter(Size = 10)] public long ProductionId { get; set; }
        public string ImageName { get; set; }

        #region Removed Dut To Save rules

        //[StoredProcedureParameter(SqlDbType = SqlDbType.Structured)]
        //public Tbl_FileDetail[] FileDetails { get; set; }

        //[IgnoreInStoredProcedureParameters] public string[] Base64Values { get; set; }

        //[IgnoreInStoredProcedureParameters] public string[] FileExtensions { get; set; }

        #endregion region   Dut To Save rules
    }
}