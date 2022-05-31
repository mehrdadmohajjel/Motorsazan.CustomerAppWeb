using System;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider
{
    public class InputAddNotificationSlider
    {
        public string Topic { get; set; }

        public string NewsText { get; set; }

        public string ImageName { get; set; }

        public DateTime ShowDateTime { get; set; }

        public DateTime EndShowDateTime { get; set; }

        public int UserId { get; set; }
    }
}