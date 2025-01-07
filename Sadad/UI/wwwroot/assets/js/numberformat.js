function formatNumbersInText() {
    // انتخاب تمام عناصر با کلاس number-format
    const elements = document.querySelectorAll('.number-format');

    elements.forEach(element => {
        // جستجوی اعداد در متن و جدا کردن آن‌ها به صورت سه‌رقمی
        element.innerHTML = element.innerHTML.replace(/\d+/g, (match) => {
            return Number(match).toLocaleString('en-US'); // جدا کردن سه‌رقمی اعداد
        });
    });
}

// اجرای تابع پس از بارگذاری کامل صفحه
window.onload = formatNumbersInText;
