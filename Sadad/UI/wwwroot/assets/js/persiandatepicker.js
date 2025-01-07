$(document).ready(function () {
    // اتصال به کلاس persian-date-picker و پیکربندی تقویم
    $('.persian-date-picker').pDatepicker({
        format: 'YYYY/MM/DD',
        autoClose: true,
        initialValue: false,
        calendarType: 'persian',
        navigator: { enabled: true, scroll: { enabled: true }, text: { btnNextText: "<", btnPrevText: ">" } },
        toolbox: { calendarSwitch: { enabled: false } },
        timePicker: { enabled: false },
        dayPicker: { enabled: true, titleFormat: 'YYYY MMMM' },
        monthPicker: { enabled: true },
        yearPicker: { enabled: true }
    });

    // تابع تبدیل اعداد انگلیسی به فارسی
    function convertToPersianNumbers(inputValue) {
        const englishNumbers = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        const persianNumbers = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

        // تبدیل اعداد به فارسی
        for (let i = 0; i < englishNumbers.length; i++) {
            inputValue = inputValue.replace(new RegExp(englishNumbers[i], 'g'), persianNumbers[i]);
        }

        return inputValue;
    }

    // اضافه کردن رویداد به input برای تبدیل اعداد به فارسی
    $('.persian-date-picker').on('input', function () {
        let value = $(this).val();
        // تبدیل اعداد به فارسی
        $(this).val(convertToPersianNumbers(value));
    });
});