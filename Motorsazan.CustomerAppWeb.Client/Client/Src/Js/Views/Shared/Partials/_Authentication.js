///<reference path="/DevExpressIntellisense/devexpress-web.d.ts" />

var motorsazanClient = motorsazanClient || {};

var controllerName = "/Base/";

motorsazanClient.loginModal = (function () {

    var dom = {};
    var isLoginAjaxAlreadyCalled = false;
    var tools = motorsazanClient.tools;


    function clearLoginModal() {
        dom.passwordInput.SetValue();
        dom.userNameInput.SetValue();

        tools.hideItem(dom.passwordInputError);
        tools.hideItem(dom.userNameInputError);
        tools.hideItem(dom.loginModalWrongCredentialsError);
    }

    function doLogin() {
        var canDoLogin = isLoginFormValid();
        if (!canDoLogin) return false;

        isLoginAjaxAlreadyCalled = true;
        var loginUrl = controllerName + "dologin";
        var loginParams = {
            userName: dom.userNameInput.GetText().trim(),
            password: dom.passwordInput.GetText().trim()
        };

        motorsazanClient.connector.post(loginUrl, loginParams)
            .then(function(loginResult) {
                isLoginAjaxAlreadyCalled = false;

                refreshNavigationBar();
                refreshShortcutBar();

                hideModal();
                clearLoginModal();
            })
            .catch(function (loginError) {
                isLoginAjaxAlreadyCalled = false;
                clearLoginModal();
                tools.showItem(dom.loginModalWrongCredentialsError);
            });
    }

    function handlePasswordInputKeyDown(source, event) {
        dom.passwordInputError.addClass("app__hide");
        if (tools.isEnterKeyPressed(event)) doLogin();
    }

    function handleUserNameInputKeyDown(source, event) {
        dom.userNameInputError.addClass("app__hide");
        if (tools.isEnterKeyPressed(event)) dom.passwordInput.Focus();
    }

    function hideModal() {
        dom.loginModal.removeClass("login__modal--active");
    }

    function init() {
        setDomItems();
        setEvents();
    }

    function isLoginFormValid() {
        var isValid = true;
        tools.hideItem(dom.userNameInputError);
        tools.hideItem(dom.passwordInputError);
        tools.hideItem(dom.loginModalWrongCredentialsError);

        if (isLoginAjaxAlreadyCalled) return false;

        if (tools.isNullOrEmpty(dom.userNameInput.GetText())) {
            dom.userNameInputError.removeClass("app__hide");
            isValid = false;
        }

        if (tools.isNullOrEmpty(dom.passwordInput.GetText())) {
            dom.passwordInputError.removeClass("app__hide");
            isValid = false;
        }

        return isValid;
    }

    function refreshShortcutBar() {
        var url = controllerName + "ShortcutBar";

        motorsazanClient.connector.post(url, {})
            .then(function (shortcutBar) {
                dom.shortcutBarParent.html(shortcutBar);
                setDomItems();
            });
    }

    function refreshNavigationBar() {
        var url = controllerName + "NavigationBar";

        motorsazanClient.connector.post(url, {})
            .then(function (navigationBar) {
                dom.navigationBarParent.html(navigationBar);
                setDomItems();
            });
    }

    function setDomItems() {
        dom.loginModal = $("#loginModal");
        dom.userNameInputError = $("#loginModalUserNameInputError");
        dom.userNameInput = ASPxClientTextBox.Cast("loginModalUserNameInput");
        dom.passwordInput = ASPxClientTextBox.Cast("loginModalPasswordInput");
        dom.passwordInputError = $("#loginModalPasswordInputError");
        dom.loginModalWrongCredentialsError = $("#loginModalWrongCredentialsError");
        dom.doLoginBtn = ASPxClientButton.Cast("loginModalDoLoginBtn");

        dom.navigationBarParent = $("#navigationBarParent");
        dom.shortcutBarParent = $("#shortcutBarParent");
    }

    function setEvents() {
        dom.userNameInput.KeyDown.AddHandler(handleUserNameInputKeyDown);
        dom.passwordInput.KeyDown.AddHandler(handlePasswordInputKeyDown);
        dom.doLoginBtn.Click.AddHandler(doLogin);
    }

    function showModal() {
        dom.loginModal.addClass("login__modal--active");
        init();
    }

    $(document).ready(init);

    return {
        hideModal: hideModal,
        showModal: showModal
    };

})();

 

function initNavigationProfile() {
    var dom = {
        miniInfo: $("#navProfileInfo"),
        profilePopup: $("#navProfilePopup")
    };

    function handlePopupOutsideClick(event) {
        var target = $(event.target);
        var popup = target.closest(".profile");
        var isClickedOutsideOfPopup = popup.length === 0;
        if (isClickedOutsideOfPopup) hidePopup();
    }

    function hidePopup() {
        dom.profilePopup.removeClass("profile__popup--active");
    }

    function setEvents() {
        dom.miniInfo.click(showPopup);
        $("body").click(handlePopupOutsideClick);
    }

    function showPopup() {
        dom.profilePopup.addClass("profile__popup--active");
    }

    setEvents(); 
}


motorsazanClient.authentication = (function () {

    function initUiComponents() {
        $(document).ready(initNavigationProfile);
    }

    function signout() {
        $.post(controllerName + "signout")
            .then(function (res) {
                location.href = res;
            })
            .catch(function (err) {
                console.log(err);
            });
    }

    initUiComponents();

    return {
        signout: signout
    };

}());