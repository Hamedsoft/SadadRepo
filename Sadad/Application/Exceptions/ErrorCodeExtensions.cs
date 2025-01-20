using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public static class ErrorCodeExtensions
    {
        public static string GetMessage(this ErrorCode code)
        {
            return code switch
            {
                ErrorCode.ModelIsNull => "اطلاعات وارد شده صحیح نیست",
                ErrorCode.InvalidOrderStatus => "وضعیت نامعتبر",
                ErrorCode.InvalidProductId => "محصول نامعتبر",
                ErrorCode.InvalidOrderId => "سفارش نامعتبر",
                _ => "خطای ناشناخته"
            };
        }

        public static string GetDescription(this ErrorCode code)
        {
            return code switch
            {
                ErrorCode.ModelIsNull => "لطفا بدنه یا پارامترهای درخواست را بررسی کنید",
                ErrorCode.InvalidOrderStatus => "شما فقط مجاز به انتخاب Status = 0 در ثبت سفارش هستید.",
                ErrorCode.InvalidProductId => "کد محصول وارد شده معتبر نیست",
                ErrorCode.InvalidOrderId => "شماره سفارش وارد شده معتبر نیست",
                _ => "لطفا با حامد ضیائی تماس بگیرید"
            };
        }
    }
}
