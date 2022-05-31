var motorsazanClient = motorsazanClient || {};
motorsazanClient.tools = (function () {

    function convertToEnNumber(value) {
        if (!value) return value;

        value = value.toString();
        value = value.replace(new RegExp("۰", "g"), "0").replace(new RegExp("٠", "g"), "0");
        value = value.replace(new RegExp("۱", "g"), "1").replace(new RegExp("١", "g"), "1");
        value = value.replace(new RegExp("۲", "g"), "2").replace(new RegExp("٢", "g"), "2");
        value = value.replace(new RegExp("۳", "g"), "3").replace(new RegExp("٣", "g"), "3");
        value = value.replace(new RegExp("۴", "g"), "4").replace(new RegExp("٤", "g"), "4");
        value = value.replace(new RegExp("۵", "g"), "5").replace(new RegExp("٥", "g"), "5");
        value = value.replace(new RegExp("۶", "g"), "6").replace(new RegExp("٦", "g"), "6");
        value = value.replace(new RegExp("۷", "g"), "7").replace(new RegExp("٧", "g"), "7");
        value = value.replace(new RegExp("۸", "g"), "8").replace(new RegExp("٨", "g"), "8");
        value = value.replace(new RegExp("۹", "g"), "9").replace(new RegExp("٩", "g"), "9");

        return value;
    }
    
    function enableItem(item) {
        item.removeAttr('disabled');
        item.removeClass('app__disable');
    }
    
    function disableItem(item) {
        item.attr('disabled', 'disabled');
        item.addClass('app__disable');
    }
    
    function hideItem(item) {
        item.addClass("app__hide");
    }

    function initDatePicker(id) {
        id = "#" + id;
        $(id).MdPersianDateTimePicker({
            targetTextSelector: id,
            targetDateSelector: id,
            enableTimePicker: false,
            textFormat: "yyyy/MM/dd",
            dateFormat: "yyyy/MM/dd",
        });
        $(id).attr("data-enabletimepicker", "false");
    }

    function initDateTimePicker(id) {
        id = "#" + id;
        $(id).MdPersianDateTimePicker({
            targetTextSelector: id,
            targetDateSelector: id,
        });
    }

    function isEnterKeyPressed(event) {
        var keyCode = event.htmlEvent.keyCode;
        var enterKeyCode = 13;
        return keyCode === enterKeyCode;
    }

    function isNullOrEmpty(value) {
        if (value === null || value === undefined) return true;
        return value.toString().trim() === "";
    }

    function isNumberOnly(value) {
        return (/^\d+$/.test(value));
    }

    function isValidBankCard(num) {
        var arr = (num + '').split('').reverse().map(function (x) {
            return parseInt(x);
        });
        var lastDigit = arr.splice(0, 1)[0];
        var sum = arr.reduce(function (acc, val, i) {
            return i % 2 !== 0 ? acc + val : acc + val * 2 % 9 || 9;
        }, 0);
        sum += lastDigit;
        return sum % 10 === 0;
    }

    function isValidEmail(email) {
        email = convertToEnNumber(email);

        if (!email || email == "") return false;

        var filter = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return filter.test(email.trim());
    }

    function isValidMobile(mobileNo) {
        mobileNo = convertToEnNumber(mobileNo);

        if (!mobileNo || mobileNo == "") return false;

        if (mobileNo.length < 10 || mobileNo.length > 11) return false;
        if (isNaN(Number(mobileNo))) return false;

        var filterWithZero = /09-?[0-9]{2}-?[0-9]{3}-?[0-9]{4}/;
        var isFilterWithZeroValid = filterWithZero.test(mobileNo.trim());

        var filterWithoutZero = /9-?[0-9]{2}-?[0-9]{3}-?[0-9]{4}/;
        var isFilterWithoutZeroValid = filterWithoutZero.test(mobileNo.trim());

        return isFilterWithZeroValid || isFilterWithoutZeroValid;
    }

    function isValidNationalCode(input) {
        input = convertToEnNumber(input);

        if (!/^\d{10}$/.test(input))
            return false;

        var check = parseInt(input[9]);
        var sum = 0;
        var i;
        for (i = 0; i < 9; ++i) {
            sum += parseInt(input[i]) * (10 - i);
        }
        sum %= 11;

        return (sum < 2 && check == sum) || (sum >= 2 && check + sum == 11);
    }

    function isValidShebaNumber(sheba) {
        function modulo(aNumStr, aDiv) {
            var tmp = "";
            var i, r;
            for (i = 0; i < aNumStr.length; i++) {
                tmp += aNumStr.charAt(i);
                r = tmp % aDiv;
                tmp = r.toString();
            }
            return tmp / 1;
        }

        function isIBAN(iban) {
            //Move front 4 digits to the end
            var rearrange =
                iban.substring(4, iban.length)
                + iban.substring(0, 4);

            var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".split('');
            var alphaMap = {};
            var number = [];
            $.each(alphabet, function (index, value) {
                alphaMap[value] = index + 10;
            });

            $.each(rearrange.split(''), function (index, value) {
                number[index] = alphaMap[value] || value;
            });

            return modulo(number.join('').toString(), 97) === 1;
        }

        return isIBAN(sheba);
    }

    function isValidPersianDate(persianDate) {
        persianDate = convertToEnNumber(persianDate ? persianDate : "");
        var filter = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})$/;
        return filter.test(persianDate);
    }

    function isValidPersianDateTime(value) {
        var persianDate = convertToEnNumber(value ? value : "");
        var filter = /^(\d{4})(\/|-)(\d{1,2})(\/|-)(\d{1,2})T(\d{2}):(\d{2})$/;
        return filter.test(persianDate);
    }

    function isValidPhoneNumber(phone) {
        phone = convertToEnNumber(phone);

        if (!phone || phone == "") return false;

        if (phone.length < 10 || phone.length > 11) return false;

        if (isNaN(Number(phone))) return false;

        var filterWithZero = /0-?[0-9]{3}-?[0-9]{3}-?[0-9]{4}/;
        var isFilterWithZeroValid = filterWithZero.test(phone.trim());

        var filterWithoutZero = /-?[0-9]{3}-?[0-9]{3}-?[0-9]{4}/;
        var isFilterWithoutZeroValid = filterWithoutZero.test(phone.trim());

        return isFilterWithZeroValid || isFilterWithoutZeroValid;
    }

    function isValidTime(value) {
        if (!value) return false;

        value = value.replace(new RegExp(":", "g"), "");

        value = value.toString().trim();
        if (value.length !== 4) return false;

        var hour = value.substr(0, 2);
        if (hour.length !== 2) return false;
        var hourAsNum = Number(hour);
        if (isNaN(hourAsNum)) return false;

        var minute = value.substr(2, 2);
        if (minute.length !== 2) return false;
        var minuteAsNum = Number(minute);
        if (isNaN(minuteAsNum)) return false;

        return true;
    }

    function lockBodyScroll() {
        $("body").addClass("body--locked");
    }

    function unLockBodyScroll() {
        $("body").removeClass("body--locked");
    }

    function showItem(item) {
        item.removeClass("app__hide");
        item.removeClass("hide");
    }

    function splitMoneyNumber(numToConvert, showRial) {
        var converted = numToConvert.toString().trim().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        if (showRial) return converted + " ریال";
        return converted;
    }

    return {
        convertToEnNumber: convertToEnNumber,
        disableItem:disableItem,
        enableItem: enableItem,
        hideItem: hideItem,
        initDatePicker: initDatePicker,
        initDateTimePicker: initDateTimePicker,
        isEnterKeyPressed: isEnterKeyPressed,
        isNumberOnly: isNumberOnly,
        isNullOrEmpty: isNullOrEmpty,
        isValidBankCard: isValidBankCard,
        isValidEmail: isValidEmail,
        isValidMobile: isValidMobile,
        isValidNationalCode: isValidNationalCode,
        isValidPersianDate: isValidPersianDate,
        isValidPersianDateTime: isValidPersianDateTime,
        isValidPhoneNumber: isValidPhoneNumber,
        isValidShebaNumber: isValidShebaNumber,
        isValidTime: isValidTime,
        lockBodyScroll: lockBodyScroll,
        splitMoneyNumber: splitMoneyNumber,
        showItem: showItem,
        unLockBodyScroll: unLockBodyScroll
    };

})();