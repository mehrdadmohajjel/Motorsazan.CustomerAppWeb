(function($) {

    var dom = null;
    var state = {
        filterTimeoutRef: null,
        systemsList: [
            {
                icon: "icon__repair--white.svg",
                name: "تعمیرات و نگهداری",
                link: "https://erp-server:2039/Home/Index",
                category: "پرتال تعمیرات و نگهداری",
                color: "danger",
                include_jwt: true
            },
            {
                icon: "icon__cms--white.svg",
                name: "درخواست خدمات رایانه‌ای",
                link: "https://erp-server:2057/",
                category: "پرتال فناوری اطلاعات",
                color: "info",
                include_jwt: false
            },
            {
                icon: "icon__gear--white.svg",
                name: "آمار ماشینکاری",
                link: "https://erp-server:2031/Home/Index",
                category: "پرتال تولید",
                color: "success",
                include_jwt: true
            },
            {
                icon: "icon__project--white.svg",
                name: "برنامه ریزی ماشینکاری",
                link: "https://erp-server:2027/Home/Index",
                category: "پرتال تولید",
                color: "gray",
                include_jwt: true
            },
            {
                icon: "icon__strategy--white.svg",
                name: "تابلو هی جونکای مونتاژ",
                link: "https://erp-server:2036/Home/Index",
                category: "پرتال تولید",
                color: "rebeccapurple",
                include_jwt: true
            },
            {
                icon: "icon__boss--white.svg",
                name: "مدیریت جلسات",
                link: "https://erp-server:2030/Home/Index",
                category: "عمومی",
                color: "warning",
                include_jwt: true
            }
        ]
    };

    function collapseDrawer() {
        dom.expandBtn.show();
        dom.drawer.removeClass("shortcutbar--expanded");
        dom.overlay.removeClass("shortcutbar__overlay--active");
    }

    function expandDrawer() {
        dom.expandBtn.hide();
        dom.drawer.addClass("shortcutbar--expanded");
        dom.overlay.addClass("shortcutbar__overlay--active");
    }

    function filterSubsystemsList() {
        var searchValue = dom.searchInput.val().trim();
        if (searchValue === "") return generateSearchResult(state.systemsList);

        var filteredList = [];

        for (var i = 0; i < state.systemsList.length; i++) {
            var tempSystem = state.systemsList[i];
            var isNameContainsSearchValue = tempSystem.name.indexOf(searchValue) > -1;
            var isCategoryContainsSearchValue = tempSystem.category.indexOf(searchValue) > -1;

            if (isNameContainsSearchValue || isCategoryContainsSearchValue)
                filteredList.push(tempSystem);
        }

        return generateSearchResult(filteredList);
    }

    function generateSearchResult(list) {
        if (list.length === 0) return dom.searchResult.html("موردی یافت نشد");

        var jwt = dom.drawer.data("jwt");
        var tempWrapper = "";
        for (var i = 0; i < list.length; i++) {
            var tempSystem = list[i];
            var classList = [
                "shortcutbar__search-link",
                "shortcutbar__search-link--" + tempSystem.color
            ];

            var systemLink = "<a class='" + classList.join(" ") + "' ";

            if (tempSystem.include_jwt) {
                systemLink += "href='" + tempSystem.link + "?MotorsazanJsonWebToken=" + jwt + "' >";
            } else {
                systemLink += "href='" + tempSystem.link + "' >";
            }

            systemLink += "<img class='shortcutbar__search-link__icon' src='/client/images/" + tempSystem.icon + "'  />";
            systemLink += tempSystem.name;
            systemLink += "<div class='shortcutbar__search-link__category'>";
            systemLink += tempSystem.category;
            systemLink += "</div>";

            systemLink += "</a>";
            
            tempWrapper += systemLink;
        }

        dom.searchResult.html(tempWrapper);
    }

    function handleSearchInputChange(event) {
        if (state.filterTimeoutRef) clearTimeout(state.filterTimeoutRef);
        state.filterTimeoutRef = setTimeout(filterSubsystemsList, 300);
    }

    function init() {
        setDom();
        setEvents();
        filterSubsystemsList();
    }

    function setDom() {
        dom = {
            drawer: $("#shortcutbarDrawer"),
            expandBtn: $("#shortcutbarExpandeBtn"),
            overlay: $("#shortcutbarOverlay"),
            searchInput: $("#shortcutbarSearchInput"),
            searchResult: $("#shortcutbarSearchResult")
        };
    }

    function setEvents() {
        dom.expandBtn.click(expandDrawer);
        dom.overlay.click(collapseDrawer);
        dom.searchInput.keyup(handleSearchInputChange);
    }

    $(document).ready(init);

})(jQuery);