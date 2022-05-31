var motorsazanClient = motorsazanClient || {};
motorsazanClient.loadingModal = (function () {

    var modalId = "#motorsazanLoadingModal";
    var activeLoadingCount = 0;

    function hideModal() {
        activeLoadingCount--;
        if (activeLoadingCount < 0) activeLoadingCount = 0;
        if (activeLoadingCount === 0) {
            $(modalId).removeClass("loading--active");
        }
    }

    function showModal(message = "در حال تکمیل فرآیند ...") {
        activeLoadingCount++;
        $("#motorsazanLoadingModal").addClass("loading--active");
        $("#motorsazanLoadingModal .loading__text").text(message);
    }

    return {
        hide: hideModal,
        show: showModal
    };

})();