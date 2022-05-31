/// <reference path="../../../Client/DevExpressIntellisense/devexpress-web.d.ts" />
/// <reference path="~/Client/Src/Js/Utilites/Tools.js" />
/// <reference path="~/Client/Src/Js/Utilites/Connector.js" />
/// <reference path="~/Client/Src/Js/Views/Shared/Components/_Uploader.js" />


window.motorsazanClient = window.motorsazanClient || {};
window.motorsazanClient.notificationSlider = (function () {

    var dom = {};
    var state = {
        machineSparePartId: null,
        notificationSliderId: 0,
        uploadedFileNamesSparePart: null,
        userFileTitleSparePart: null,
        uploaderReferenceSparePart: null,
        catalogUploadedFileNamesSparePart: null,
        catalogUserFileTitleSparePart: null,
        persianShowDateTime: "",
        persianEndShowDateTime: "",
        oldImageName: "",
        fileData: "",
        fileName: "",
        fileUrl: "",
        fileSize: "",
        imageName: null
    };

    var tools = motorsazanClient.tools;
    var controllerName = "/NotificationSlider/";

    async function fillGrid() {

        const url = controllerName + "NotificationSliderGrid";

        const apiParam = {
            productionCategoryId: state.productionCategoryId
        };


        const gridHtml = await motorsazanClient.connector.post(url, apiParam);

        dom.gridParent.html(gridHtml);
        setDom();
    }

    async function handleAddNews() {

        if (!isAddFormValid()) return false;
        var fileUpload = $("#imgUp").get(0);
        var files = fileUpload.files;

        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        fileData.append('file', files);
        fileData.append('Topic', dom.addFormTopicTextBox.GetValue());
        fileData.append('NewsText', dom.addFormNewsTextTextBox.GetValue());
        fileData.append('PersianShowDateTime', dom.addFormPeriodStartDatePicker.val());
        fileData.append('PersianEndShowDateTime', dom.addFormPeriodEndDatePicker.val());
        $.ajax({
            url: '/NotificationSlider/AddNotificationSlider',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data  
            data: fileData,
            success: function (result) {
                 fillGrid();
                motorsazanClient.messageModal.success(result);
                resetAddForm();
            },
            error: function (err) {
                motorsazanClient.messageModal.success(result);
                 fillGrid();
            }
        });

        //const url = controllerName + "AddNotificationSlider";
        //const apiParam = {
        //    Topic: dom.addFormTopicTextBox.GetValue(),
        //    NewsText: dom.addFormNewsTextTextBox.GetValue(),
        //    ImageName: state.uploadedFileNamesSparePart,
        //    PersianShowDateTime: dom.addFormPeriodStartDatePicker.val(),
        //    PersianEndShowDateTime: dom.addFormPeriodEndDatePicker.val()
        //};

        //const apiResult = await motorsazanClient.connector.post(url, apiParam);
        //await fillGrid();
        //motorsazanClient.messageModal.success(apiResult);
        //resetAddForm();
    }

    async function handleEditNews() {


            var fileUpload = $("#imgUp").get(0);
            var files = fileUpload.files;

            var fileData = new FormData();

            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            fileData.append('file', files);

            fileData.append('notificationSliderId', state.notificationSliderId);
            fileData.append('Topic', dom.addFormTopicTextBox.GetValue());
            fileData.append('NewsText', dom.addFormNewsTextTextBox.GetValue());
            fileData.append('PersianShowDateTime', dom.addFormPeriodStartDatePicker.val());
            fileData.append('PersianEndShowDateTime', dom.addFormPeriodEndDatePicker.val());
            fileData.append('oldImageName', state.oldImageName);

            $.ajax({
                url: '/NotificationSlider/EditNotificationSlider',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                     fillGrid();
                    motorsazanClient.messageModal.success(result);
                    resetAddForm();

                },
                error: function (err) {
                    motorsazanClient.messageModal.success(result);
                     fillGrid();

                }
            });
       

    }

    function handleCancel() {
        resetAddForm();

    }

    function handleAddFormTopicTextBoxValueChanged() {
        tools.hideItem(dom.addFormTopicTextBoxError);
    }

    function handleAddFormNewsTextTextBoxValueChanged() {
        tools.hideItem(dom.addFormNewsTextTextBoxError);
    }



    async function showAddnotificationSliderImageBtnForm() {

        const url = controllerName + "AddnotificationSliderImagePartial";
        const apiParam = {
            productionId: state.productionId
        };
        const title = "افزودن تصویر محصول";
        motorsazanClient.contentModal.ajaxShow(title, url, apiParam, setUploaderDom);
        await fillGrid();

    }


    function handleNotificationSliderGridCallbackUrl(command) {
        const url = controllerName + "NotificationSliderGrid";
        command.callbackUrl = url + "?persianShowDateTime=" + state.persianShowDateTime + "&persianEndShowDateTime=" + state.persianEndShowDateTime;

    }


    function resetAddForm() {

        dom.addFormTopicTextBox.SetText('');
        dom.addFormNewsTextTextBox.SetText('');
        dom.addFormPeriodStartDatePicker.val("");
        dom.addFormPeriodEndDatePicker.val("");
        state.uploadedFileNames = null;
        state.userFileTitle = null;

        //Clear uploader without deleting uploaded files
        state.uploaderReferenceSparePart.clear(false);
        tools.hideItem(dom.editDiv);
        tools.showItem(dom.saveDive);

    }

    function isAddFormValid() {
        var isValid = true;


        tools.hideItem(dom.addFormTopicTextBoxError);
        var TopicText = dom.addFormTopicTextBox.GetValue();
        var isTopicTextValid = !tools.isNullOrEmpty(TopicText);
        if (!isTopicTextValid) {
            isValid = false;
            tools.showItem(dom.addFormTopicTextBoxError);
        }

        tools.hideItem(dom.addFormNewsTextTextBoxError);
        var NewsText = dom.addFormNewsTextTextBox.GetValue();
        var isNewsTextValid = !tools.isNullOrEmpty(NewsText);
        if (!isNewsTextValid) {
            isValid = false;
            tools.showItem(dom.addFormNewsTextTextBoxError);
        }


        tools.hideItem(dom.addFormPeriodStartDatePickerError);
        tools.hideItem(dom.addFormPeriodEndDatePickerError);

        const periodStart = dom.addFormPeriodStartDatePicker.val();
        const isPeriodStartSelected = !tools.isNullOrEmpty(periodStart);
        if (!isPeriodStartSelected) {
            isValid = false;
            tools.showItem(dom.addFormPeriodStartDatePickerError);
        }

        const periodEnd = dom.addFormPeriodEndDatePicker.val();
        const isPeriodEndSelected = !tools.isNullOrEmpty(periodEnd);
        if (!isPeriodEndSelected) {
            isValid = false;
            tools.showItem(dom.addFormPeriodEndDatePickerError);
        }
        //tools.hideItem(dom.notificationSliderImageAttachmentUploaderError);
        //   if (state.userFileTitleSparePart == null) {
        //   tools.showItem(dom.notificationSliderImageAttachmentUploaderError);
        //       return false;
        //   }
        return isValid;
    }


    function setDom() {

        dom.addFormTopicTextBoxParent = $("#addFormTopicTextBoxParent");
        dom.addFormTopicTextBoxError = $("#addFormTopicTextBoxError");
        dom.addFormTopicTextBox = ASPxClientTextBox.Cast("addFormTopicTextBox");

        dom.addFormNewsTextTextBoxParent = $("#addFormNewsTextTextBoxParent");
        dom.addFormNewsTextTextBoxError = $("#addFormNewsTextTextBoxError");
        dom.addFormNewsTextTextBox = ASPxClientMemo.Cast("addFormNewsTextTextBox");


        dom.addFormPeriodStartDatePicker = $("#addFormPeriodStartDatePicker");
        dom.addFormPeriodStartDatePickerParent = $("#addFormPeriodStartDatePickerParent");
        dom.addFormPeriodStartDatePickerError = $("#addFormPeriodStartDatePickerError");

        dom.addFormPeriodEndDatePicker = $("#addFormPeriodEndDatePicker");
        dom.addFormPeriodEndDatePickerParent = $("#addFormPeriodEndDatePickerParent");
        dom.addFormPeriodEndDatePickerError = $("#addFormPeriodEndDatePickerError");


        dom.filterFormBeingItemsDivCenter = $("#filterFormBeingItemsDivCenter");

        dom.saveDive = $("#saveDive");
        dom.editDiv = $("#editDiv");


        dom.gridParent = $("#gridParent");
        dom.grid = ASPxClientGridView.Cast("grid");

        dom.ucMultiSelection = ASPxClientUploadControl.Cast("UploadControl");

    }
    function setUploaderDom() {
        dom.notificationSliderImageAttachmentUploader = $("#notificationSliderImageAttachmentUploader");
        dom.notificationSliderImageAttachmentUploaderError = $("#notificationSliderImageAttachmentUploaderError");
        dom.notificationSliderImageAttachmentUploaderParent = $("#notificationSliderImageAttachmentUploaderParent");

        addImageToNotificationSlider();

    }


    function init() {
        setDom();
        //setUploaderDom();
        setEvents();
    }

    $(document).ready(init);

    function handleAddFormPeriodEndDatePickerSelectionChange() {
        tools.hideItem(dom.addFormPeriodEndDatePickerError);
    }

    function handleAddFormPeriodStartDatePickerSelectionChange() {
        tools.hideItem(dom.addFormPeriodStartDatePickerError);
    }

    function handleGridCustomBtnClick(source, event) {
        setDom();
        state.notificationSliderId =
            dom.grid.GetRowKey(event.visibleIndex);
        const customBtnId = event.buttonID;

        const rowIndex = event.visibleIndex;

        source.GetRowValues(rowIndex,
            ["NotificationSliderId;ImageName"],
            async (values) => {
                const [notificationSliderId, imageName] = values;
                state.notificationSliderId = notificationSliderId;
                state.imageName = imageName;

            });
        
        if (customBtnId === "editTopicBTN") {

            setDefaultValuesOfEdit();
        }
        if (customBtnId === "removeTopicBTN") {

             removeTopicRow(source, event);
}

    }
    function removeTopicRow() {
        var title = "تاییدیه حذف";
        var content = "آیا از حذف این ردیف مطمئن هستید؟";
        motorsazanClient.messageModal.confirm(content, removeTopic, title);

    }

    async function setDefaultValuesOfEdit() {
        const url = "/notificationSlider/GetNotificationSliderListByNotificationSliderId";
        const apiParam = {
            notificationSliderId: state.notificationSliderId

        };
        const apiResult = await motorsazanClient.connector.post(url, apiParam);
        tools.hideItem(dom.saveDive);
        tools.showItem(dom.editDiv);

        state.notificationSliderId = apiResult.NotificationSliderId;

        dom.addFormTopicTextBox.SetText(apiResult.Topic);
        dom.addFormNewsTextTextBox.SetText(apiResult.NewsText);
        dom.addFormPeriodStartDatePicker.val(apiResult.PersianShowDateTime);
        dom.addFormPeriodEndDatePicker.val(apiResult.PersianEndShowDateTime);
        state.userFileTitleSparePart = apiResult.ImageName;
        state.oldImageName = apiResult.ImageName;
    }

    async function removeTopic(values) {
        const imageName = state.imageName;
        var params = {
            notificationSliderId: state.notificationSliderId,
            imageName: imageName
        };
        const url = controllerName + "RemoveNotificationSliderByNotificationSliderId";
        const result = await motorsazanClient.connector.post(url, params);
        await fillGrid();
        motorsazanClient.messageModal.success(result);
        motorsazanClient.messageModal.hide();

    }

    function setEvents() {
        dom.addFormPeriodStartDatePicker.change(handleAddFormPeriodStartDatePickerSelectionChange);
        dom.addFormPeriodEndDatePicker.change(handleAddFormPeriodEndDatePickerSelectionChange);
    }
    function addImageToNotificationSlider() {
        setDom();
        state.uploaderReferenceSparePart = null;
        state.uploadedFileNamesSparePart = null;
        state.userFileTitleSparePart = null;

        state.uploaderReferenceSparePart = new motorsazanClient.uploader().create({
            accept: "image/*",
            id: "notificationSliderImageAttachmentUploader",
            uploadCallback: uploadImageToNotificationSlider,
            deleteUploadedFilesCallback: removeUploadedAttachmentsOfMachineSparePart,
            maxSize: 1 * 1024 * 1024, // 1 GB
            isMultipleSelection: true,
            multipleSelectionConfig: {
                newNameForSelectedFilesMode: 1 //Required For All
            }
        });

    }

    async function removeUploadedAttachmentsOfMachineSparePart(fileNames) {
        const url = controllerName + "RemoveUploadedAttachmentsOfMachineSparePart";


        const fileNamesToSend = fileNames.map(fileName => { return { fileName } });

        const apiParam = {
            productionId: state.productionId,
            fileNames: fileNamesToSend
        };

        await motorsazanClient.connector.post(url, apiParam);

        state.uploadedFileNamesSparePart = null;
        state.userFileTitleSparePart = null;
    }

    function showUploadedImageToProduct(values) {
        const fileName = values;

        const downloadAddresses = (`${controllerName}GetNotificationSliderAttachmentFileByFileName?fileName=${fileName}`);


        window.open(downloadAddresses, "_blank");
    }

    async function filterNotificationSlider() {
        await fillGrid();
    }

    async function uploadImageToNotificationSlider(base64Values, fileNames) {
        const url = controllerName + "AddImageToNotificationSlider";
        setDom();
        setUploaderDom();
        const extensions = fileNames.map(fileName => `.${fileName.split(".")[1]}`);
        const apiParam = {
            base64Values: base64Values,
            fileExtensions: extensions
        };

        const uploadedFileNamesSparePart = await motorsazanClient.connector.post(url, apiParam);

        const fileTitles = fileNames.map(fileName => `${fileName.split(".")[0]}`);

        motorsazanClient.contentModal.hideAllModal();

        if (state.uploadedFileNamesSparePart && state.uploadedFileNamesSparePart.length > 0) {
            state.uploadedFileNamesSparePart.push(...uploadedFileNamesSparePart);
            state.userFileTitleSparePart.push(...fileTitles);
        } else {
            state.userFileTitleSparePart = fileTitles;
            state.uploadedFileNamesSparePart = uploadedFileNamesSparePart;
        }


        //For use in deleting files
        state.uploaderReferenceSparePart.saveNameOfUploadedFiles(state.uploadedFileNamesSparePart);
        const attachmentDownloadAddresses = state.uploadedFileNamesSparePart.map
            (fileName => `${controllerName}GetNotificationSliderAttachmentFileByFileName?fileName=${fileName}`);
        state.uploaderReferenceSparePart.showUploadedFileDemoLinksForMultiSelection(fileNames,
            attachmentDownloadAddresses);


    }
    function clearUploader() {

    }
    function onFileUploadComplete(e) {
        if (e.callbackData) {
            var fileData = e.callbackData.split('|');
            var fileName = fileData[0],
                fileUrl = fileData[1],
                fileSize = fileData[2];
        }
    }


    return {
        handleNotificationSliderGridCallbackUrl: handleNotificationSliderGridCallbackUrl,
        handleAddFormTopicTextBoxValueChanged: handleAddFormTopicTextBoxValueChanged,
        handleAddFormNewsTextTextBoxValueChanged: handleAddFormNewsTextTextBoxValueChanged,
        handleAddNews: handleAddNews,
        handleGridCustomBtnClick: handleGridCustomBtnClick,
        handleCancel: handleCancel,
        handleEditNews: handleEditNews,
        filterNotificationSlider: filterNotificationSlider,
        clearUploader: clearUploader,
        onFileUploadComplete: onFileUploadComplete
    };

})();