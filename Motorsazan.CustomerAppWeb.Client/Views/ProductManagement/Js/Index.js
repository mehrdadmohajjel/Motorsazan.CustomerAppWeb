/// <reference path="../../../Client/DevExpressIntellisense/devexpress-web.d.ts" />
/// <reference path="~/Client/Src/Js/Utilites/Tools.js" />
/// <reference path="~/Client/Src/Js/Utilites/Connector.js" />
/// <reference path="~/Client/Src/Js/Views/Shared/Components/_Uploader.js" />


window.motorsazanClient = window.motorsazanClient || {};
window.motorsazanClient.productManagement = (function () {

    var dom = {};
    var state = {
        machineSparePartId: null,
        productionCategoryId: 0,
        productionId:0,
        uploadedFileNamesSparePart: null,
        userFileTitleSparePart: null,
        uploaderReferenceSparePart: null,
        catalogUploaderReferenceSparePart: null,
        catalogUploadedFileNamesSparePart: null,
        catalogUserFileTitleSparePart: null,
        imageName: "",
        catalogName:""
    };

    var tools = motorsazanClient.tools;
    var controllerName = "/ProductManagement/";

    async function fillGrid() {

        const url = controllerName + "ProductManagementGrid";

        const apiParam = {
            productionCategoryId: state.productionCategoryId
        };


        const gridHtml = await motorsazanClient.connector.post(url, apiParam);

        dom.gridParent.html(gridHtml);
        setDom();
    }

    function handleAddImage() {
        var fileUpload = $("#imgUp").get(0);
        var files = fileUpload.files;

        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        fileData.append('file', files);
        fileData.append('productionId', state.productionId);
        $.ajax({
            url: '/ProductManagement/AddImageToProduction',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data  
            data: fileData,
            success: function (result) {
                fillGrid();
                motorsazanClient.contentModal.hideModal();
                motorsazanClient.messageModal.success(result);
            },
            error: function (err) {
                motorsazanClient.contentModal.hideModal();
                motorsazanClient.messageModal.success(err);
                fillGrid();
            }
        });
    }


    function handleAddCatalog() {
        var fileUpload = $("#calaogUp").get(0);
        var files = fileUpload.files;

        var fileData = new FormData();

        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        fileData.append('file', files);
        fileData.append('productionId', state.productionId);
        $.ajax({
            url: '/ProductManagement/AddCatalogToProduction',
            type: "POST",
            contentType: false, // Not to set any content header
            processData: false, // Not to process data  
            data: fileData,
            success: function (result) {
                motorsazanClient.contentModal.hideModal();
                fillGrid();
                motorsazanClient.messageModal.success(result);
            },
            error: function (err) {
                motorsazanClient.contentModal.hideModal();
                motorsazanClient.messageModal.success(err);
                fillGrid();
            }
        });
    }

    async function handleAddProduct() {

        if (!isAddFormValid()) return false;
        const url = controllerName + "AddProduct";
        const apiParam = {
        ProductionCategoryId: addFormProductCategoryCombo.GetValue(),
        EngineType : dom.addFormEngineTypeTextBox.GetValue(),
            NumberOfCylinder: dom.addFormNumberOfCylinder.GetValue(),
            ApproximateWeight: dom.addFormApproximateWeightTextBox.GetValue(),
            MaxPower: dom.addFormMaxPowerTextBox.GetValue(),
            MaxTorque: dom.addFormMaxTorqueTextBox.GetValue(),
            Length: dom.addFormLengthTextBox.GetValue(),
            Width: dom.addFormWidthTextBox.GetValue(),
            Height: dom.addFormHeightTextBox.GetValue()
        };

        const apiResult = await motorsazanClient.connector.post(url, apiParam);
        await fillGrid();
        motorsazanClient.messageModal.success(apiResult);
        resetAddForm();
        return false;
    }

    function handleGridCustomBtnClick(source, event) {
        setDom();
        state.productionId =
            dom.grid.GetRowKey(event.visibleIndex);

        const rowIndex = event.visibleIndex;

        source.GetRowValues(rowIndex,
            ["ProductionId;ImageName;CatalogName"],
            async (values) => {
                const [productionId, imageName, catalogName] = values;
                state.productionId = productionId;
                state.imageName = imageName;
                state.catalogName= catalogName;

            });



        const customBtnId = event.buttonID;

        if (customBtnId === "addProductImageBtn") { return showAddProductImageBtnForm(); }
        if (customBtnId === "addProductCatalogBtn") { return showAddProductCatalogBtnForm(); }
        if (customBtnId === "getUploadedMachineDocumentMemberBtn") {
            dom.grid.GetRowValues(
                dom.grid.GetFocusedRowIndex(),
                "ImageName",
                (values) => {
                    showUploadedImageToProduct(values);
                }
            );
        }

        if (customBtnId === "getUploadedcatalogCustomButton") {
            dom.grid.GetRowValues(
                dom.grid.GetFocusedRowIndex(),
                "CatalogName",
                (values) => {
                    showUploadedCatalog(values);
                }
            );
        }

        if (customBtnId === "removeProductBTN") { return removeProductRow(source, event); }

    }

    function handleAddFormNumberOfCylinderValueChanged() {
        tools.hideItem(dom.addFormNumberOfCylinderError);
    }
    function handleAddFormEngineTypeTextBoxValueChanged() {
        tools.hideItem(dom.addFormEngineTypeError);
    }
    function handleAddFormApproximateWeightTextBoxValueChanged() {
        tools.hideItem(dom.addFormApproximateWeightError);
    }
    function handleAddFormMaxPowerTextBoxChanged() {
        tools.hideItem(dom.addFormMaxPowerError);
    }
    function handleAddFormMaxTorqueTextBoxChanged() {
        tools.hideItem(dom.addFormMaxTorqueError);
    }
    function handleAddFormLengthTextBoxChanged() {
        tools.hideItem(dom.addFormLengthError);
    }
    function handleAddFormWidthTextBoxChanged() {
        tools.hideItem(dom.addFormWidthError);
    }
    function handleAddFormHeightTextBoxChanged() {
        tools.hideItem(dom.addFormHeightError);
    }
    
     function removeProductRow() {
        var title = "تاییدیه حذف";
        var content = "آیا از حذف این ردیف مطمئن هستید؟";

        motorsazanClient.messageModal.confirm(content, removeProduction, title);

    }

    async function removeProduction(values) {

        var params = {
            productionId: state.productionId
        };
        const url = controllerName + "RemoveProductionByProductionId";
        const result = await motorsazanClient.connector.post(url, params);
        await fillGrid();
        motorsazanClient.messageModal.success(result);
        motorsazanClient.messageModal.hide();

    }

      async  function showAddProductImageBtnForm() {
          
        const url = controllerName + "AddProductImagePartial";
        const apiParam = {
            productionId: state.productionId
        };
        const title = "افزودن تصویر محصول";
          motorsazanClient.contentModal.ajaxShow(title, url, apiParam, setUploaderDom);
          await fillGrid();

      }

    function showAddProductCatalogBtnForm() {
        const url = controllerName + "AddProductCatalogPartial";
        const apiParam = {
            productionId: state.productionId
        };

        const title = "افزودن کاتالوگ";
        motorsazanClient.contentModal.ajaxShow(title, url, apiParam, setCatalogUploaderDom);
        fillGrid();
    }

    function handleProductManagementGridCallbackUrl(command) {
        const url = controllerName + "ProductManagementGrid";
        command.callbackUrl = url + "?ProductionCategoryId=" + state.ProductionCategoryId;
       
    }
    function handleAddFormProductCategoryComboChanged() {
        state.productionCategoryId = addFormProductCategoryCombo.GetValue();
        dom.addFormEngineTypeTextBox.SetEnabled(true);
        dom.addFormNumberOfCylinder.SetEnabled(true);
        dom.addFormApproximateWeightTextBox.SetEnabled(true);
        dom.addFormMaxPowerTextBox.SetEnabled(true);
        dom.addFormMaxTorqueTextBox.SetEnabled(true);
        dom.addFormLengthTextBox.SetEnabled(true);
        dom.addFormWidthTextBox.SetEnabled(true);
        dom.addFormHeightTextBox.SetEnabled(true);
        tools.hideItem(dom.addFormProductCategoryComboError);
         fillGrid();

    }

    function resetAddForm() {

        dom.addFormEngineTypeTextBox.SetEnabled(false);
        dom.addFormNumberOfCylinder.SetEnabled(false);
        dom.addFormApproximateWeightTextBox.SetEnabled(false);
        dom.addFormMaxPowerTextBox.SetEnabled(false);
        dom.addFormMaxTorqueTextBox.SetEnabled(false);
        dom.addFormLengthTextBox.SetEnabled(false);
        dom.addFormWidthTextBox.SetEnabled(false);
        dom.addFormHeightTextBox.SetEnabled(false);


        dom.addFormProductCategoryCombo.SetSelectedIndex(-1);
        dom.addFormNumberOfCylinder.SetText('');
        dom.addFormEngineTypeTextBox.SetText('');
        dom.addFormApproximateWeightTextBox.SetText('');
        dom.addFormMaxPowerTextBox.SetText('');
        dom.addFormMaxTorqueTextBox.SetText('');
        dom.addFormLengthTextBox.SetText('');
        dom.addFormWidthTextBox.SetText('');
        dom.addFormHeightTextBox.SetText('');

    }

    function isAddFormValid() {
        var isValid = true;

        tools.hideItem(dom.addFormProductCategoryComboError);
        var Category = dom.addFormProductCategoryCombo.GetValue();
        var isCategoryValid = !tools.isNullOrEmpty(Category);
        if (!isCategoryValid) {
            isValid = false;
            tools.showItem(dom.addFormProductCategoryComboError);
        }


        tools.hideItem(dom.addFormEngineTypeError);
        var EngineType = dom.addFormEngineTypeTextBox.GetValue();
        var isEngineTypeValid = !tools.isNullOrEmpty(EngineType);
        if (!isEngineTypeValid) {
            isValid = false;
            tools.showItem(dom.addFormEngineTypeError);
        }

        tools.hideItem(dom.addFormNumberOfCylinderError);
        var NumberOfCylinder = dom.addFormNumberOfCylinder.GetValue();
        var isNumberOfCylinderValid = !tools.isNullOrEmpty(NumberOfCylinder);
        if (!isNumberOfCylinderValid) {
            isValid = false;
            tools.showItem(dom.addFormNumberOfCylinderError);
        }

        tools.hideItem(dom.addFormApproximateWeightError);
        var ApproximateWeight = dom.addFormApproximateWeightTextBox.GetValue();
        var isApproximateWeightValid = !tools.isNullOrEmpty(ApproximateWeight);
        if (!isApproximateWeightValid) {
            isValid = false;
            tools.showItem(dom.addFormApproximateWeightError);
        }

        tools.hideItem(dom.addFormMaxPowerError);
        var MaxPower = dom.addFormMaxPowerTextBox.GetValue();
        var isMaxPowerValid = !tools.isNullOrEmpty(MaxPower);
        if (!isMaxPowerValid) {
            isValid = false;
            tools.showItem(dom.addFormMaxPowerError);
        }

        tools.hideItem(dom.addFormMaxTorqueError);
        var MaxTorque = dom.addFormMaxTorqueTextBox.GetValue();
        var isMaxTorqueValid = !tools.isNullOrEmpty(MaxTorque);
        if (!isMaxTorqueValid) {
            isValid = false;
            tools.showItem(dom.addFormMaxTorqueError);
        }

        tools.hideItem(dom.addFormLengthError);
        var Lenght = dom.addFormLengthTextBox.GetValue();
        var isLenghtValid = !tools.isNullOrEmpty(Lenght);
        if (!isLenghtValid) {
            isValid = false;
            tools.showItem(dom.addFormLengthError);
        }

        tools.hideItem(dom.addFormWidthError);
        var Width = dom.addFormWidthTextBox.GetValue();
        var isWidthValid = !tools.isNullOrEmpty(Width);
        if (!isWidthValid) {
            isValid = false;
            tools.showItem(dom.addFormWidthError);
        }

        tools.hideItem(dom.addFormHeightError);
        var Height = dom.addFormHeightTextBox.GetValue();
        var isHeightValid = !tools.isNullOrEmpty(Height);
        if (!isHeightValid) {
            isValid = false;
            tools.showItem(dom.addFormHeightError);
        }

        return isValid;
    }


    function setDom() {
        dom.addFormProductCategoryComboParent = $("#addFormProductCategoryComboParent");
        dom.addFormProductCategoryComboError = $("#addFormProductCategoryComboError");
        dom.addFormProductCategoryCombo = ASPxClientComboBox.Cast("addFormProductCategoryCombo");

        dom.addFormEngineTypeParent = $("#addFormEngineTypeParent");
        dom.addFormEngineTypeError = $("#addFormEngineTypeError");
        dom.addFormEngineTypeTextBox = ASPxClientTextBox.Cast("addFormEngineTypeTextBox");

        dom.addFormNumberOfCylinderParent = $("#addFormNumberOfCylinderParent");
        dom.addFormNumberOfCylinderError = $("#addFormNumberOfCylinderError");
        dom.addFormNumberOfCylinder = ASPxClientSpinEdit.Cast("addFormNumberOfCylinder");

        dom.addFormApproximateWeightParent = $("#addFormApproximateWeightParent");
        dom.addFormApproximateWeightError = $("#addFormApproximateWeightError");
        dom.addFormApproximateWeightTextBox = ASPxClientTextBox.Cast("addFormApproximateWeightTextBox");

        dom.addFormMaxPowerParent = $("#addFormMaxPowerParent");
        dom.addFormMaxPowerError = $("#addFormMaxPowerError");
        dom.addFormMaxPowerTextBox = ASPxClientTextBox.Cast("addFormMaxPowerTextBox");

        dom.addFormMaxTorqueParent = $("#addFormMaxTorqueParent");
        dom.addFormMaxTorqueError = $("#addFormMaxTorqueError");
        dom.addFormMaxTorqueTextBox = ASPxClientTextBox.Cast("addFormMaxTorqueTextBox");

        dom.addFormLengthParent = $("#addFormLengthParent");
        dom.addFormLengthError = $("#addFormLengthError");
        dom.addFormLengthTextBox = ASPxClientTextBox.Cast("addFormLengthTextBox");

        dom.addFormWidthParent = $("#addFormWidthParent");
        dom.addFormWidthError = $("#addFormWidthError");
        dom.addFormWidthTextBox = ASPxClientTextBox.Cast("addFormWidthTextBox");

        dom.addFormHeightParent = $("#addFormHeightParent");
        dom.addFormHeightError = $("#addFormHeightError");
        dom.addFormHeightTextBox = ASPxClientTextBox.Cast("addFormHeightTextBox");

        dom.gridParent = $("#gridParent");
        dom.grid = ASPxClientGridView.Cast("grid");

    }
    function setUploaderDom()
    {
        dom.productImageAttachmentUploaderError = $("#productImageAttachmentUploaderError");
        dom.productImageAttachmentUploaderParent = $("#productImageAttachmentUploaderParent");


    }
    function setCatalogUploaderDom() {

        dom.productCatalogAttachmentUploaderError = $("#productCatalogAttachmentUploaderError");
        dom.productCatalogAttachmentUploaderParent = $("#productCatalogAttachmentUploaderParent");


    }

   function init() {
      setDom();
    }

    $(document).ready(init);




    function showUploadedImageToProduct(values) {
        const fileName = values;
        const productionId = state.productionId;

        const downloadAddresses = ('/FileBank/Production/Images/' + fileName);


        window.open(downloadAddresses, "_blank");
    }


    function showUploadedCatalog(values) {
        const CatalogName = values;
        const productionId = state.productionId;

        const downloadAddresses = ('/FileBank/Production/Catalog/' + CatalogName);


        window.open(downloadAddresses, "_blank");
    }



    return {
        handleProductManagementGridCallbackUrl: handleProductManagementGridCallbackUrl,
        handleAddFormProductCategoryComboChanged: handleAddFormProductCategoryComboChanged,
        handleAddProduct: handleAddProduct,
        handleGridCustomBtnClick: handleGridCustomBtnClick,
        handleAddFormEngineTypeTextBoxValueChanged: handleAddFormEngineTypeTextBoxValueChanged,
        handleAddFormNumberOfCylinderValueChanged: handleAddFormNumberOfCylinderValueChanged,
        handleAddFormApproximateWeightTextBoxValueChanged: handleAddFormApproximateWeightTextBoxValueChanged,
        handleAddFormMaxPowerTextBoxChanged: handleAddFormMaxPowerTextBoxChanged,
        handleAddFormMaxTorqueTextBoxChanged: handleAddFormMaxTorqueTextBoxChanged,
        handleAddFormLengthTextBoxChanged: handleAddFormLengthTextBoxChanged,
        handleAddFormWidthTextBoxChanged: handleAddFormWidthTextBoxChanged,
        handleAddFormHeightTextBoxChanged: handleAddFormHeightTextBoxChanged,
        handleAddImage: handleAddImage,
        handleAddCatalog: handleAddCatalog
    };

})();