using System;
using Motorsazan.CustomerAppWeb.Shared.Attributes;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider
{
    public class InputEditNotificationSlider
    {
        public long NotificationSliderId { get; set; }

        public string Topic { get; set; }

        public string NewsText { get; set; }

        [IgnoreInStoredProcedureParameters] public string[] FileStream { get; set; }

        public string ImageName { get; set; }

        public DateTime ShowDateTime { get; set; }

        public DateTime EndShowDateTime { get; set; }

        public int UserId { get; set; }

        public string OldImageName { get; set; }
    }
}