using Motorsazan.CustomerAppWeb.Shared.Attributes;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider
{
    public class InputGetNotificationSliderAttachmentFileByFileName
    {
        [StoredProcedureParameter(Size = 200)] public string FileName { get; set; }
    }
}