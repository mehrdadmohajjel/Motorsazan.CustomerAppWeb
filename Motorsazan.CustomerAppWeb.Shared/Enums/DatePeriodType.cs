using System.ComponentModel.DataAnnotations;

namespace Motorsazan.CustomerAppWeb.Shared.Enums
{
    public enum DatePeriodType
    {
        [Display(Name = "بازه زمانی خاص")] CustomPeriod,

        [Display(Name = "از ابتدای سال")] ThisYear,

        [Display(Name = "شش ماه اخیر")] RecentSixMonths,

        [Display(Name = "سه ماه اخیر")] RecentThreeMonths,

        [Display(Name = "یک ماه اخیر")] RecentMonth,

        [Display(Name = "یک هفته اخیر")] RecentWeek,

        [Display(Name = "روز قبل")] Yesterday,

        [Display(Name = "روزجاری")] CurrentDay
    }
}