using Motorsazan.CustomerAppWeb.Api.Business;
using Motorsazan.CustomerAppWeb.Api.Filters;
using Motorsazan.CustomerAppWeb.Shared.Models.DomainModels;
using Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider;
using Motorsazan.CustomerAppWeb.Shared.Models.Output.NotificationSlider;
using System;
using System.Linq;
using System.Web.Http;

namespace Motorsazan.CustomerAppWeb.Api.Controllers
{
    [RoutePrefix("NotificationSlider")]
    public class NotificationSliderController: ApiController
    {
        private readonly BusinessManager _businessManager = new BusinessManager();


        /// <summary>
        ///     آپلود عکس محصول
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [Route("AddImageToNotificationSlider")]
        [HttpPost]
        [JwtValidation]
        public IHttpActionResult AddImageToProduction(InputAddImageToNotificationSlider values)
        {
            const string storedProcedureName = "[Sale].[prc_AddImageToNotificationSlider]";

            values.FileDetails = values.FileExtensions.Select(
                (fileExtension, i) =>
                    new Tbl_FileDetail
                    {
                        FileName = "NotificationSlider-" + Guid.NewGuid().ToString().Substring(0, 4) +
                                   fileExtension,
                        ImageStream = Convert.FromBase64String(values.Base64Values[i])
                    }
            ).ToArray();

            _businessManager.CallStoredProcedure(storedProcedureName, values);

            return Ok(values.FileDetails.Select(fileDetail => fileDetail.FileName));
        }


        /// <summary>
        ///     افزودن اعلان جدید
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [JwtValidation]
        [Route("AddNotificationSlider")]
        [HttpPost]
        public IHttpActionResult AddNotificationSlider(InputAddNotificationSlider input)
        {
            const string storedProcedureName = "[Sale].[prc_AddNotificationSlider]";

            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, input);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "اعلان با موفقیت اضافه شد";
            return Ok(message);
        }

        /// <summary>
        ///     ویرایش اعلان اسلایدر
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [JwtValidation]
        [Route("EditNotificationSlider")]
        [HttpPost]
        public IHttpActionResult EditNotificationSlider(InputEditNotificationSlider input)
        {
            const string storedProcedureName = "[Sale].[prc_EditNotificationSlider]";

            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, input);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "اعلان با موفقیت ویرایش شد";
            return Ok(message);
        }


        /// <summary>
        ///     دریافت آخرین خبر
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetLastNews")]
        [HttpPost]
        public IHttpActionResult GetLastNews()
        {
            const string storedProcedureName = "[Sale].[prc_GetLastNews]";

            var result =
                _businessManager
                    .CallStoredProcedure<
                        OutputGetLastNews[]>(
                        storedProcedureName);

            return Ok(result);
        }

        /// <summary>
        ///     دریافت 5اعلان آخر
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetLastFiveNews")]
        [HttpPost]
        public IHttpActionResult GetLastFiveNews()
        {
            const string storedProcedureName = "[Sale].[prc_GetLastFiveNews]";

            var result =
                _businessManager
                    .CallStoredProcedure<
                        OutputGetLastNews[]>(
                        storedProcedureName);

            return Ok(result);
        }


        /// <summary>
        ///     دریافت لیست اخبار بر اساس شرایط
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetNotificationSliderListByCondition")]
        [HttpPost]
        public IHttpActionResult GetNotificationSliderListByCondition(InputGetNotificationSliderListByCondition input)
        {
            const string storedProcedureName = "[Sale].[prc_GetNotificationSliderListByCondition]";

            var result =
                _businessManager
                    .CallStoredProcedure<InputGetNotificationSliderListByCondition,
                        OutputGetNotificationSliderListByCondition[]>(
                        storedProcedureName, input);

            return Ok(result);
        }

        /// <summary>
        ///     دریافت جزئیات اعلان با شناسه اعلان
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetNotificationSliderListByNotificationSliderId")]
        [HttpPost]
        public IHttpActionResult GetNotificationSliderListByNotificationSliderId(
            InputGetNotificationSliderListByNotificationSliderId input)
        {
            const string storedProcedureName = "[Sale].[prc_GetNotificationSliderListByNotificationSliderID]";

            var result =
                _businessManager
                    .CallStoredProcedure<InputGetNotificationSliderListByNotificationSliderId,
                        OutputGetNotificationSliderListByNotificationSliderId>(
                        storedProcedureName, input);

            return Ok(result);
        }


        /// <summary>
        ///     دریافت جزئیات اعلان با شناسه اعلان
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("GetNotificationSliderAttachmentFileByFileName")]
        [HttpPost]
        public IHttpActionResult GetNotificationSliderAttachmentFileByFileName(
            InputGetNotificationSliderAttachmentFileByFileName input)
        {
            const string storedProcedureName = "[Sale].[prc_GetNotificationSliderAttachmentFileByFileName]";

            var result =
                _businessManager
                    .CallStoredProcedure<InputGetNotificationSliderAttachmentFileByFileName,
                        OutputGetNotificationSliderAttachmentFileByFileName>(
                        storedProcedureName, input);

            return Ok(result);
        }

        /// <summary>
        ///     حذف اعلان اسلایدر بر اساس شناسه جدول اعلان اسلایدر
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestModelNullValidation]
        [RequestModelValidation]
        [Route("RemoveNotificationSliderByNotificationSliderId")]
        [HttpPost]
        public IHttpActionResult RemoveNotificationSliderByNotificationSliderId(
            InputRemoveNotificationSliderByNotificationSliderId input)
        {
            const string storedProcedureName = "[Sale].[prc_RemoveNotificationSliderByNotificationSliderID]";

            var message = _businessManager.CallStoredProcedureAndReturnMessageIfExits(storedProcedureName, input);

            if(!string.IsNullOrEmpty(message))
            {
                return BadRequest(message);
            }

            message = "اعلان انتخابی حذف شد";
            return Ok(message);
        }
    }
}