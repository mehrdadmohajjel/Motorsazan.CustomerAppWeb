using Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement;
using Motorsazan.CustomerAppWeb.Shared.Models.Output.ProductManagement;
using System.Threading.Tasks;

namespace Motorsazan.CustomerAppWeb.Client.Api
{
    public static partial class ApiList
    {
        public static string AddProduct(InputAddProduct input, string token)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(AddProduct);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(url, methodName, token, input)
            );

            return task.GetAwaiter().GetResult();
        }


        public static string RemoveProductionByProductionId(InputRemoveProductionByProductionId input, string token)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(RemoveProductionByProductionId);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(url, methodName, token, input)
            );

            return task.GetAwaiter().GetResult();
        }

        public static OutputGetProductCategoryList[] GetProductCategoryList()
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(GetProductCategoryList);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetProductCategoryList[]>.Post(
                        url,
                        methodName, parameters: null)
            );

            return task.GetAwaiter().GetResult();
        }

        public static OutputGetProductionListByProductCategoryId[] GetProductionListByProductCategoryId(
            InputGetProductionListByProductCategoryId input, string token)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(GetProductionListByProductCategoryId);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetProductionListByProductCategoryId[]>.Post(
                        url,
                        methodName, parameters: input, token: token)
            );

            return task.GetAwaiter().GetResult();
        }

        public static string AddImageToProduction(InputAddImageToProduction values, string token)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(AddImageToProduction);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(
                        url,
                        methodName, parameters: values, token: token)
            );

            return task.GetAwaiter().GetResult();
        }

        public static string AddCatalogToProduction(InputAddCatalogToProduction values, string token)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(AddCatalogToProduction);

            var task = Task.Run(
                async () =>
                    await ApiConnector<string>.Post(
                        url,
                        methodName, parameters: values, token: token)
            );

            return task.GetAwaiter().GetResult();
        }


        public static OutputGetProductionAttachmentFileByProductionId GetProductionAttachmentFileByProductionId(
            InputGetProductionAttachmentFileByProductionId input)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(GetProductionAttachmentFileByProductionId);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetProductionAttachmentFileByProductionId>.Post(
                        url,
                        methodName, parameters: input)
            );

            return task.GetAwaiter().GetResult();
        }

        public static OutputGetProductionCatalogAttachmentFileByProductionId
            GetProductionCatalogAttachmentFileByProductionId(
                InputGetProductionCatalogAttachmentFileByProductionId input)
        {
            var url = $"{BaseUrl}/ProductManagement/";
            const string methodName = nameof(GetProductionCatalogAttachmentFileByProductionId);

            var task = Task.Run(
                async () =>
                    await ApiConnector<OutputGetProductionCatalogAttachmentFileByProductionId>.Post(
                        url,
                        methodName, parameters: input)
            );

            return task.GetAwaiter().GetResult();
        }
    }
}