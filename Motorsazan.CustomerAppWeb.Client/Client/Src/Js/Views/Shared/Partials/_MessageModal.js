///<reference path="/DevExpressIntellisense/devexpress-web.d.ts" />

var motorsazanClient = motorsazanClient || {};
motorsazanClient.messageModal = (function () {

    var dom = {
        modal: "#messageModal",
        title: "#messageModalTitle",
        closeBtn: "#messageModalCloseBtn",
        contentWrapper: "#mesageModalContentWrapper",
        confirmActionBtn: "messageModalConfirmActionBtn",
        cancelActionBtn: "messageModalCancelActionBtn",
        icon: "#messageModalIcon"
    }
    var confirmBtnCallbackRef = null;
    var isEventAlreadySetted = false;

    function handleKeyPressByUser(event) {
        const escKeyCode = 27;
        const enterKeyCode = 13;

        const isEscPressed = event.keyCode === escKeyCode;
        const isEnterPressed = event.keyCode === enterKeyCode;

        if (isEscPressed) {
            hideModal();
            $('body').off('keydown');
        }
        else if (isEnterPressed) {
            runConfirmCallbackRef();
            $('body').off('keydown');
        }
    }

    function hideModal() {
        $(dom.modal).removeClass("message-modal--active");

        // Re-Focus on modals if there is no active content modal, tries to 
        // focus on messageModal if the message modal is not displaying then
        // finally focuses on body tag
        $("body").focus();
        $("#messageModal[class*=active]").focus();
        $("*[id*=contentModal_][class*=active]").focus();
    }

    function runConfirmCallbackRef() {
        hideModal();
        if (confirmBtnCallbackRef) confirmBtnCallbackRef();
    }

    function showErrorMessage(content, title) {
        title = title || "بروز خطا";
        var errorIcon = "cancel.svg";
        showMessage(content, title, errorIcon);
    }

    function setEvents() {
        $(dom.closeBtn).click(hideModal);

        var cancelBtn = ASPxClientButton.Cast(dom.cancelActionBtn);
        cancelBtn.Click.AddHandler(hideModal);

        var confirmBtn = ASPxClientButton.Cast(dom.confirmActionBtn);
        confirmBtn.Click.AddHandler(runConfirmCallbackRef);
    }

    function showConfirm(content, confirmCallback, title) {
        title = title || "دریافت تاییدیه اقدام";
        confirmBtnCallbackRef = confirmCallback;

        var confirmBtn = ASPxClientButton.Cast(dom.confirmActionBtn);
        confirmBtn.SetClientVisible(true);

        showModal(content, title, "انصراف", "warning.svg");
    }

    function showMessage(content, title, icon) {
        var confirmBtn = ASPxClientButton.Cast(dom.confirmActionBtn);
        confirmBtn.SetClientVisible(false);

        showModal(content, title, "بستن پیام", icon);
    }

    function showModal(content, title, cancelText, iconFile) {
        $(dom.title).html(title);
        $(dom.contentWrapper).html(content);

        $(dom.modal).addClass("message-modal--active");
        var iconUrl = "/Client/Images/" + iconFile;
        $(dom.icon).attr("src", iconUrl);

        if (!isEventAlreadySetted) setEvents();
        isEventAlreadySetted = true;

        var cancelBtn = ASPxClientButton.Cast(dom.cancelActionBtn);
        cancelBtn.SetText(cancelText);

        $("body").keydown(handleKeyPressByUser);

        $("#messageModal").focus();
    }

    function showSuccessMessage(content, title) {
        title = title || "پیام سامانه";
        var successIcon = "checked.svg";
        showMessage(content, title, successIcon);
    }

    function showWarningMessage(content, title) {
        title = title || "هشدار";
        var errorIcon = "warning.svg";
        showMessage(content, title, errorIcon);
    }


    return {
        confirm: showConfirm,
        error: showErrorMessage,
        hide: hideModal,
        success: showSuccessMessage,
        warning: showWarningMessage
    };

})();