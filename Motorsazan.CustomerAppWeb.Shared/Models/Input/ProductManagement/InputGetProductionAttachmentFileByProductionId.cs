using Motorsazan.CustomerAppWeb.Shared.Attributes;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement
{
    public class InputGetProductionAttachmentFileByProductionId
    {
        [StoredProcedureParameter(Size = 10)] public string ProductionId { get; set; }

        [StoredProcedureParameter(Size = 200)] public string FileName { get; set; }
    }
}