namespace Motorsazan.CustomerAppWeb.Shared.Models.Output.NotificationSlider
{
    public class OutputGetLastNews
    {
        public long NotificationSliderId { get; set; }

        public string Topic { get; set; }

        public string NewsText { get; set; }

        public string FileStream { get; set; }

        public string ImageName { get; set; }
    }
}