using DevExpress.Web;
using Motorsazan.CustomerAppWeb.Client.Api;
using Motorsazan.CustomerAppWeb.Shared.Models.Input.NotificationSlider;
using Motorsazan.CustomerAppWeb.Shared.Models.Output.NotificationSlider;
using Motorsazan.CustomerAppWeb.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Motorsazan.CustomerAppWeb.Client.Controllers
{
    public class NotificationSliderController: BaseController
    {
        public string NotificationSliderPath { get; set; } = Settings.NotificationSliderPath;


        public ActionResult AddNotificationSlider(string topic, string newsText,
            string persianShowDateTime, string persianEndShowDateTime)
        {
            var files = Request.Files;
            HttpPostedFileBase file1;
            var input = new InputAddNotificationSlider {Topic = topic, NewsText = newsText};

            for(var i = 0; i < files.Count; i++)
            {
                file1 = files[i];

                // Checking for Internet Explorer  
                string fname;
                if(Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    var testfiles = file1.FileName.Split('\\');
                    fname = testfiles[testfiles.Length - 1];
                    input.ImageName = Guid.NewGuid() + fname;
                    fname = Path.Combine(Server.MapPath(NotificationSliderPath), input.ImageName);
                    file1.SaveAs(fname);
                }
                else
                {
                    fname = file1.FileName;
                    input.ImageName = Guid.NewGuid() + fname;
                    fname = Path.Combine(Server.MapPath(NotificationSliderPath), input.ImageName);
                    file1.SaveAs(fname);
                }
            }


            var token = GetUserToken();
            input.ShowDateTime = Tools.ConvertToLatinDate(persianShowDateTime);
            input.EndShowDateTime = Tools.ConvertToLatinDate(persianEndShowDateTime);
            input.UserId = GetCurrentUser().UserID;

            var apiResult = ApiList.AddNotificationSlider(input, token);
            return Content(apiResult);
        }


        public ActionResult EditNotificationSlider(long notificationSliderId, string topic, string newsText,
            string persianShowDateTime, string persianEndShowDateTime, string oldImageName)
        {
            var files = Request.Files;
            HttpPostedFileBase file1;
            var input = new InputEditNotificationSlider
            {
                Topic = topic,
                NewsText = newsText,
                OldImageName = oldImageName,
                NotificationSliderId = notificationSliderId
            };
            for(var i = 0; i < files.Count; i++)
            {
                file1 = files[i];

                string fname;
                // Checking for Internet Explorer  
                if(Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    var testfiles = file1.FileName.Split('\\');
                    fname = testfiles[testfiles.Length - 1];
                    input.ImageName = Guid.NewGuid() + fname;
                    fname = Path.Combine(Server.MapPath(NotificationSliderPath), input.ImageName);
                    file1.SaveAs(fname);
                }
                else
                {
                    fname = file1.FileName;
                    input.ImageName = Guid.NewGuid() + fname;
                    fname = Path.Combine(Server.MapPath(NotificationSliderPath), input.ImageName);
                    file1.SaveAs(fname);
                }
            }


            var token = GetUserToken();
            input.ShowDateTime = Tools.ConvertToLatinDate(persianShowDateTime);
            input.EndShowDateTime = Tools.ConvertToLatinDate(persianEndShowDateTime);
            input.UserId = GetCurrentUser().UserID;
            var apiResult = ApiList.EditNotificationSlider(input, token);
            return Content(apiResult);
        }

        public ActionResult AddImageToNotificationSlider(InputAddImageToNotificationSlider input)
        {
            var token = GetUserToken();

            var apiResult = ApiList.AddImageToNotificationSlider(input, token);

            return Json(apiResult);
        }


        public ActionResult GetNotificationSliderAttachmentFileByFileName(
            InputGetNotificationSliderAttachmentFileByFileName input)
        {
            var apiResult = ApiList.GetNotificationSliderAttachmentFileByFileName(input);

            var fileBytes = Convert.FromBase64String(apiResult.ImageFile);

            return File(fileBytes, MediaTypeNames.Application.Octet, apiResult.ImageName);
        }

        public ActionResult Index() => View();

        public ActionResult NotificationSliderGrid(string persianShowDateTime, string persianEndShowDateTime)
        {
            const string partialViewUrl = "~/Views/NotificationSlider/Grid/NotificationSliderGrid.cshtml";

            var apiParam = new InputGetNotificationSliderListByCondition
            {
                StartDate =
                    !string.IsNullOrEmpty(persianShowDateTime)
                        ? Tools.ConvertToLatinDate(persianShowDateTime)
                        : DateTime.Now.AddDays(-7),
                EndDate = !string.IsNullOrEmpty(persianEndShowDateTime)
                    ? Tools.ConvertToLatinDate(persianShowDateTime)
                    : DateTime.Now
            };
            var dataSource = ApiList.GetNotificationSliderListByCondition(apiParam);
            var result = new List<OutputGetNotificationSliderListByCondition>();
            if(dataSource.Length != 0)
            {
                foreach(var item in dataSource)
                {
                    var input = new OutputGetNotificationSliderListByCondition
                    {
                        //FileStream = item.FileStream,
                        //ImageBinary = Convert.FromBase64String(item.FileStream),
                        ImageName = item.ImageName,
                        NewsText = item.NewsText,
                        NotificationSliderId = item.NotificationSliderId,
                        PersianEndShowDateTime = item.PersianEndShowDateTime,
                        PersianShowDateTime = item.PersianShowDateTime,
                        Topic = item.Topic,
                        UserName = item.UserName
                    };
                    result.Add(input);
                }
            }

            return PartialView(partialViewUrl, result);
        }

        public ActionResult ImageUploader()
        {
            const string partialViewUrl = "~/Views/NotificationSlider/AddForm/ImageUploader.cshtml";

            return PartialView(partialViewUrl);
        }

        public void FileUploadCompleteMultiSelect(object sender, FileUploadCompleteEventArgs e)
        {
            var uploadDirectory = "~/FileBank/NotificationSlider/";

            var resultFileUrl = uploadDirectory + e.UploadedFile.FileName;
            var resultFilePath = Request.MapPath(resultFileUrl);

            e.UploadedFile.SaveAs(resultFilePath);

            Tools.RemoveFileWithDelay(e.UploadedFile.FileName, resultFilePath, 5);

            if(sender is IUrlResolutionService urlResolver)
            {
                var url = urlResolver.ResolveClientUrl(resultFileUrl);
                e.CallbackData = GetCallbackData(e.UploadedFile, url);
            }
        }

        public static string GetCallbackData(UploadedFile uploadedFile, string fileUrl)
        {
            var name = uploadedFile.FileName;
            var sizeInKilobytes = uploadedFile.ContentLength / 1024;
            var sizeText = sizeInKilobytes + " KB";

            return name + "|" + fileUrl + "|" + sizeText;
        }


        public ActionResult RemoveNotificationSliderByNotificationSliderId(
            InputRemoveNotificationSliderByNotificationSliderId input, string imageName)
        {
            var token = GetUserToken();
            if(imageName != null)
            {
                System.IO.File.Delete(Server.MapPath("~/FileBank/NotificationSlider/" + imageName));
            }

            var apiResult = ApiList.RemoveNotificationSliderByNotificationSliderId(input, token);
            return Content(apiResult);
        }

        public JsonResult GetNotificationSliderListByNotificationSliderId(
            InputGetNotificationSliderListByNotificationSliderId values)
        {
            var inspectionDetailDataSource = ApiList.GetNotificationSliderListByNotificationSliderId(values);

            return inspectionDetailDataSource != null ? Json(inspectionDetailDataSource) : Json("");
        }
    }
}