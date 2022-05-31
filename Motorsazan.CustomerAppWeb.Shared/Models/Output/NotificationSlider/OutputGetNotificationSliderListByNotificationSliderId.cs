namespace Motorsazan.CustomerAppWeb.Shared.Models.Output.NotificationSlider
{
    public class OutputGetNotificationSliderListByNotificationSliderId
    {
        public long NotificationSliderId { get; set; }

        public string Topic { get; set; }

        public string NewsText { get; set; }

        public string PersianShowDateTime { get; set; }

        public string PersianEndShowDateTime { get; set; }

        public string FileStream { get; set; }

        public string ImageName { get; set; }
    }
}