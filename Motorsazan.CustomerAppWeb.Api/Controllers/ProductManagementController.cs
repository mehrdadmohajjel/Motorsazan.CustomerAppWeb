using Motorsazan.CustomerAppWeb.Api.Business;
using Motorsazan.CustomerAppWeb.Api.Filters;
using Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement;
using Motorsazan.CustomerAppWeb.Shared.Models.Output.ProductManagement;
using System.Web.Http;

namespace Motorsazan.CustomerAppWeb.Api.Controllers
{
    [RoutePrefix("ProductManagement")]
    public class ProductManagementController: ApiController
    {
        private readonly BusinessManager _businessManager = new BusinessManager();


        /// <summary>
        ///     دریافت لیست گروه های محصولات
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("AddProduct")]
        [HttpPost]
        public IHttpActionResult AddProduct(InputAddProduct input)
        {
            const string storedProcedureName = "[Sale].[prc_AddProduct]";

            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, input);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "محصول با موفقیت اضافه شد";
            return Ok(message);
        }


        /// <summary>
        ///     دریافت لیست گروه های محصولات
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetProductCategoryList")]
        [HttpPost]
        public IHttpActionResult GetProductCategoryList(
        )
        {
            const string storedProcedureName = "[Sale].[prc_GetProductCategoryList]";

            var result =
                _businessManager
                    .CallStoredProcedure<
                        OutputGetProductCategoryList[]>(
                        storedProcedureName);

            return Ok(result);
        }

        /// <summary>
        ///     دریافت لسیت محصولات بر اساس شناسه جدول گروه بندی محصولات
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetProductionListByProductCategoryID")]
        [HttpPost]
        public IHttpActionResult GetProductionListByProductCategoryID(InputGetProductionListByProductCategoryId input)
        {
            const string storedProcedureName = "[Sale].[prc_GetProductionListByProductCategoryID]";

            var result =
                _businessManager
                    .CallStoredProcedure<InputGetProductionListByProductCategoryId,
                        OutputGetProductionListByProductCategoryId[]>(
                        storedProcedureName, input);

            return Ok(result);
        }

        /// <summary>
        ///     آپلود عکس محصول
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [Route("AddImageToProduction")]
        [HttpPost]
        [JwtValidation]
        public IHttpActionResult AddImageToProduction(InputAddImageToProduction values)
        {
            const string storedProcedureName = "[Sale].[prc_AddImageUploadAttachmentToProduct]";

            #region SaveDB

            //values.FileDetails = values.FileExtensions.Select(
            //    (fileExtension, i) =>
            //        new Tbl_FileDetail
            //        {
            //            FileName = values.ProductionId + "-" + Guid.NewGuid().ToString().Substring(0, 4) +
            //                       fileExtension,
            //            ImageStream = Convert.FromBase64String(values.Base64Values[i])
            //        }
            //).ToArray();

            #endregion SaveDB

            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, values);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "محصول با موفقیت اضافه شد";
            return Ok(message);
        }

        /// <summary>
        ///     آپلود کانالوگ محصول
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [Route("AddCatalogToProduction")]
        [HttpPost]
        [JwtValidation]
        public IHttpActionResult AddCatalogToProduction(InputAddCatalogToProduction values)
        {
            const string storedProcedureName = "[Sale].[prc_AddCatalogUploadAttachmentToProduct]";


            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, values);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "محصول با موفقیت اضافه شد";
            return Ok(message);
        }


        /// <summary>
        ///     دریافت عکس محصول
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetProductionAttachmentFileByProductionId")]
        [HttpPost]
        //[AccessToEventValidation(EventCode = "003", FormCode = "002")]
        public IHttpActionResult GetProductionAttachmentFileByProductionId(
            InputGetProductionAttachmentFileByProductionId input)
        {
            const string storedProcedureName = "[Sale].[prc_GetProductionAttachmentFileByProductionID]";

            var result =
                _businessManager
                    .CallStoredProcedure<InputGetProductionAttachmentFileByProductionId,
                            OutputGetProductionAttachmentFileByProductionId>
                        (storedProcedureName, input);

            return Ok(result);
        }

        /// <summary>
        ///     حذف آپلود ضمیمه های محصول
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("RemoveProductionByProductionId")]
        [HttpPost]
        public IHttpActionResult RemoveProductionByProductionId(InputRemoveProductionByProductionId input)
        {
            const string storedProcedureName = "[Sale].[prc_RemoveProductionByProductionID]";

            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, input);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "محصول با موفقیت اضافه شد";
            return Ok(message);
        }
    }
}