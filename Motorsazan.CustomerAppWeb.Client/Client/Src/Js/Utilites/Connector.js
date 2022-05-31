var motorsazanClient = motorsazanClient || {};
motorsazanClient.connector = (function () {

    function get(url, data) {
        var getPromiseProxy = new Promise(function (resolve, reject) {
            motorsazanClient.loadingModal.show();
            $.get(url, data)
                .then(function (pureResult) {
                   motorsazanClient.loadingModal.hide();
                   resolve(pureResult);
                })
                .catch(function (err) {
                    parseError(err)
                    reject(err);
                });
        });
        return getPromiseProxy;
    }

    function parseError(err) {
        motorsazanClient.loadingModal.hide();
        var code = err.status;
        if (code == 401) {
            motorsazanClient.loginModal.showModal();
        }
        else if (code == 403) {
            motorsazanClient.messageModal.error("شما مجوز دسترسی به این عملیات را ندارید");
        }
        else {
            motorsazanClient.messageModal.error(err.responseJSON.Error);
        }
    }

    function post(url, data) {
        var postPromiseProxy = new Promise(function (resolve, reject) {
            motorsazanClient.loadingModal.show();
            $.post(url, data)
                .then(function (pureResult) {
                    motorsazanClient.loadingModal.hide();
                    resolve(pureResult)
                })
                .catch(function (err) {
                    parseError(err)
                    reject(err);
                });
        });
        return postPromiseProxy;
    }

    return {
        get: get,
        post: post
    };

})();