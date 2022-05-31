(function() {

    function hidePopup(element) {
        element.removeClass("app__section__settings__popup--active");
    }

    function init() {
        setEvents();
    }

    function setEvents() {
        var appSections = $(".app__section");

        appSections.each(function (index, currentSection) {

            var settingPopup = $($.find(".app__section__settings__popup", currentSection)[0]);
            var settingBtn = $($.find(".app__section__settings__menu-btn", currentSection)[0]);
            var header = $($.find(".app__section__header", currentSection)[0]);

            settingBtn.on("click", function () {
                settingPopup.toggleClass("app__section__settings__popup--active");
            });

            var jqueryCurrentSection = $(currentSection);

            var maximizeBtn = $($.find(".app__section__settings__maximize-btn", currentSection)[0]);
            maximizeBtn.on("click", function () {
                jqueryCurrentSection.removeClass("app__section--minimized");
                jqueryCurrentSection.toggleClass("app__section--maximized");
                hidePopup(settingPopup);
            });

            var minimizeBtn = $($.find(".app__section__settings__minimize-btn", currentSection)[0]);
            minimizeBtn.on("click", function () {
                jqueryCurrentSection.removeClass("app__section--maximized");
                jqueryCurrentSection.toggleClass("app__section--minimized");
                hidePopup(settingPopup);
            });

            header.click(function (event) {
                var isHeaderClicked = $(event.target).hasClass("app__section__header");
                if (isHeaderClicked) {
                    jqueryCurrentSection.removeClass("app__section--maximized");
                    jqueryCurrentSection.toggleClass("app__section--minimized");
                    hidePopup(settingPopup);
                }
            });

            

        });

        $("body").click(function(event) {

            var isClickedOnSettingArea = $(event.target).parents(".app__section__settings");
            if (isClickedOnSettingArea.length === 0) {
                $(".app__section__settings__popup--active").removeClass("app__section__settings__popup--active");
            }
        });
    }

    
    $(document).ready(init);

})();