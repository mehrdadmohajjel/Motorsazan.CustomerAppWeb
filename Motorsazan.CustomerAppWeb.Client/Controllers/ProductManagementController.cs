using Motorsazan.CustomerAppWeb.Client.Api;
using Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement;
using Motorsazan.CustomerAppWeb.Shared.Utilities;
using System;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Motorsazan.CustomerAppWeb.Client.Controllers
{
    public class ProductManagementController: BaseController
    {
        public string ProductionCatalogPath { get; set; } = Settings.ProductionCatalogPath;
        public string ProductionImagePath { get; set; } = Settings.ProductionImagePath;

        public ActionResult AddProduct(InputAddProduct input)
        {
            var token = GetUserToken();
            var apiResult = ApiList.AddProduct(input, token);
            return Content(apiResult);
        }

        public ActionResult RemoveProductionByProductionId(InputRemoveProductionByProductionId input)
        {
            var token = GetUserToken();
            var apiResult = ApiList.RemoveProductionByProductionId(input, token);
            return Content(apiResult);
        }

        public ActionResult ProductManagementGrid(long productionCategoryId = 0)
        {
            const string partialViewUrl = "~/Views/ProductManagement/Grid/ProductManagementGrid.cshtml";

            var apiParam = new InputGetProductionListByProductCategoryId {ProductCategoryId = productionCategoryId};
            var token = GetUserToken();
            var dataSource = ApiList.GetProductionListByProductCategoryId(apiParam, token);
            return PartialView(partialViewUrl, dataSource);
        }


        public ActionResult AddFormProductCategoryCombo()
        {
            const string partialViewUrl =
                "~/Views/ProductManagement/AddForm/AddFormProductCategoryCombo.cshtml";
            var currentUserDepartmentListDataSource = ApiList.GetProductCategoryList();
            return PartialView(partialViewUrl, currentUserDepartmentListDataSource);
        }

        public ActionResult Index() => View();

        public ActionResult AddImageToProduction(long productionId)
        {
            var input = new InputAddImageToProduction {ProductionId = productionId};

            var files = Request.Files;
            HttpPostedFileBase file;

            for(var i = 0; i < files.Count; i++)
            {
                file = files[i];

                string imageName;
                // Checking for Internet Explorer  
                if(Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    var testfiles = file.FileName.Split('\\');
                    imageName = testfiles[testfiles.Length - 1];
                    input.ImageName = Guid.NewGuid().ToString().Substring(0, 5) + imageName;
                    imageName = Path.Combine(Server.MapPath(ProductionImagePath), input.ImageName);
                    file.SaveAs(imageName);
                }
                else
                {
                    imageName = file.FileName;
                    input.ImageName = Guid.NewGuid().ToString().Substring(0, 5) + imageName;
                    imageName = Path.Combine(Server.MapPath(ProductionImagePath), input.ImageName);
                    file.SaveAs(imageName);
                }
            }


            var token = GetUserToken();
            var apiResult = ApiList.AddImageToProduction(input, token);
            return Content(apiResult);
        }

        public ActionResult AddProductImagePartial()
        {
            const string partialViewUrl =
                "~/Views/ProductManagement/AddForm/AddProductImagePartial.cshtml";
            return PartialView(partialViewUrl);
        }

        public ActionResult AddProductCatalogPartial()
        {
            const string partialViewUrl =
                "~/Views/ProductManagement/AddForm/AddProductCatalogPartial.cshtml";
            return PartialView(partialViewUrl);
        }

        public ActionResult AddCatalogToProduction(long productionId)
        {
            var input = new InputAddCatalogToProduction {ProductionId = productionId};

            var files = Request.Files;
            HttpPostedFileBase file;

            for(var i = 0; i < files.Count; i++)
            {
                file = files[i];

                string catalogName;
                // Checking for Internet Explorer  
                if(Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    var testfiles = file.FileName.Split('\\');
                    catalogName = testfiles[testfiles.Length - 1];
                    input.CatalogName = Guid.NewGuid().ToString().Substring(0, 5) + catalogName;
                    catalogName = Path.Combine(Server.MapPath(ProductionCatalogPath), input.CatalogName);
                    file.SaveAs(catalogName);
                }
                else
                {
                    catalogName = file.FileName;
                    input.CatalogName = Guid.NewGuid().ToString().Substring(0, 5) + catalogName;
                    catalogName = Path.Combine(Server.MapPath(ProductionCatalogPath), input.CatalogName);
                    file.SaveAs(catalogName);
                }
            }

            var token = GetUserToken();

            var apiResult = ApiList.AddCatalogToProduction(input, token);

            return Content(apiResult);
        }


        public ActionResult GetProductionAttachmentFileByProductionId(
            InputGetProductionAttachmentFileByProductionId input)
        {
            var apiResult = ApiList.GetProductionAttachmentFileByProductionId(input);

            var fileBytes = Convert.FromBase64String(apiResult.ImageFile);

            return File(fileBytes, MediaTypeNames.Application.Octet, apiResult.ImageName);
        }

        public ActionResult GetProductionCatalogAttachmentFileByProductionId(
            InputGetProductionAttachmentFileByProductionId input)
        {
            var apiResult = ApiList.GetProductionAttachmentFileByProductionId(input);

            var fileBytes = Convert.FromBase64String(apiResult.CatalogFile);

            return File(fileBytes, MediaTypeNames.Application.Octet, apiResult.CatalogName);
        }
    }
}