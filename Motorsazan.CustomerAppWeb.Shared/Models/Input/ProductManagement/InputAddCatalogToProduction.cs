using Motorsazan.CustomerAppWeb.Shared.Attributes;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement
{
    public class InputAddCatalogToProduction
    {
        [StoredProcedureParameter(Size = 10)] public long ProductionId { get; set; }
        public string CatalogName { get; set; }


        #region

        //[StoredProcedureParameter(SqlDbType = SqlDbType.Structured)]
        //public Tbl_FileDetail[] FileDetails { get; set; }

        //[IgnoreInStoredProcedureParameters] public string[] Base64Values { get; set; }

        //[IgnoreInStoredProcedureParameters] public string[] FileExtensions { get; set; }

        #endregion
    }
}