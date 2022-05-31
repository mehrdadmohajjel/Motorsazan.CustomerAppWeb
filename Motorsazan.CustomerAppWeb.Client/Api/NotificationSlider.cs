using Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider;
using Motorsazan.CustomerAppWeb.Shared.Models.Output.NotificationSlider;
using System.Threading.Tasks;

namespace Motorsazan.CustomerAppWeb.Client.Api
{
    public partial class ApiList
    {
        public static string AddNotificationSlider(InputAddNotificationSlider input, string token)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(AddNotificationSlider);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(url, methodName, token, input)
            );

            return task.GetAwaiter().GetResult();
        }

        public static string EditNotificationSlider(InputEditNotificationSlider input, string token)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(EditNotificationSlider);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(url, methodName, token, input)
            );

            return task.GetAwaiter().GetResult();
        }

        public static string[] AddImageToNotificationSlider(InputAddImageToNotificationSlider values, string token)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(AddImageToNotificationSlider);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string[]>.Post(
                        url,
                        methodName, parameters: values, token: token)
            );

            return task.GetAwaiter().GetResult();
        }

        public static OutputGetNotificationSliderAttachmentFileByFileName GetNotificationSliderAttachmentFileByFileName(
            InputGetNotificationSliderAttachmentFileByFileName input)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(GetNotificationSliderAttachmentFileByFileName);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetNotificationSliderAttachmentFileByFileName>.Post(
                        url,
                        methodName, parameters: input)
            );

            return task.GetAwaiter().GetResult();
        }

        public static OutputGetNotificationSliderListByNotificationSliderId
            GetNotificationSliderListByNotificationSliderId(
                InputGetNotificationSliderListByNotificationSliderId input)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(GetNotificationSliderListByNotificationSliderId);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetNotificationSliderListByNotificationSliderId>.Post(
                        url,
                        methodName, parameters: input)
            );

            return task.GetAwaiter().GetResult();
        }


        public static OutputGetNotificationSliderListByCondition[] GetNotificationSliderListByCondition(
            InputGetNotificationSliderListByCondition input)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(GetNotificationSliderListByCondition);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetNotificationSliderListByCondition[]>.Post(
                        url,
                        methodName, parameters: input)
            );

            return task.GetAwaiter().GetResult();
        }

        public static string RemoveNotificationSliderByNotificationSliderId(
            InputRemoveNotificationSliderByNotificationSliderId input, string token)
        {
            var url = $"{BaseUrl}/NotificationSlider/";
            const string methodName = nameof(RemoveNotificationSliderByNotificationSliderId);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(url, methodName, token, input)
            );

            return task.GetAwaiter().GetResult();
        }
    }
}