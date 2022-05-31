function initNavigationBar() {
    var dom = {
        nav: $("#nav"),
        showMenuBtn: $("#navShowMenuBtn"),
        drawer: $("#navDrawer"),
        overlay: $("#navOverlay")
    };

    function handleMenuItemsToggle(event) {
        const target = $(event.target);

        if ($("#navShowMenuBtn").css("display") === "none") return;

        const isClickedItemTypeIsMenuRootItem = target.hasClass("nav__item--has-submenu");
        if (isClickedItemTypeIsMenuRootItem) target.toggleClass("nav__item--expanded");

        const isParentOfSecondLevelSubMenuClicked = target.hasClass("nav__item--has-second-level-submenu");
        if (isParentOfSecondLevelSubMenuClicked) {
            target.toggleClass("nav__item--has-second-level-submenu--expanded");
            const secondLevelSubMenu = target.children(".nav__second-level-submenu");
            secondLevelSubMenu.toggleClass("nav__second-level-submenu--expanded");

            target.css("margin-bottom", 0);

            if (secondLevelSubMenu.hasClass("nav__second-level-submenu--expanded")) {

                const numberOfLinks = secondLevelSubMenu.children().toArray()
                    .filter(a => $(a).hasClass("nav__link")).length;

                const bottomMargin = numberOfLinks * 30 + (numberOfLinks > 0 ? 10 : 0);
                target.css("margin-bottom", bottomMargin);
            }
        }
    }

    function hideDrawer() {
        dom.drawer.removeClass("nav__drawer--active");
        dom.overlay.removeClass("nav__overlay--active");
        location.hash = "";
    }

    function setEvents() {
        dom.showMenuBtn.click(showDrawer);
        dom.overlay.click(hideDrawer);
        window.addEventListener("hashchange", hideDrawer);
        dom.drawer.click(handleMenuItemsToggle);
    }

    function showDrawer() {
        dom.drawer.addClass("nav__drawer--active");
        dom.overlay.addClass("nav__overlay--active");
        if (history.pushState) history.pushState(null, null, "#navigation");
        else location.hash = '#navigation';
    }


    setEvents();
}

$(document).ready(initNavigationBar);