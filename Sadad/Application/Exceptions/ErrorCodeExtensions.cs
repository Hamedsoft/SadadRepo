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
                _ => "خطای ناشناخته"
            };
        }

        public static string GetDescription(this ErrorCode code)
        {
            return code switch
            {
                ErrorCode.ModelIsNull => "لطفا بدنه یا پارامترهای درخواست را بررسی کنید",
                ErrorCode.InvalidOrderStatus => "شما فقط مجاز به انتخاب Status = 0 در ثبت سفارش هستید.",
                _ => "لطفا با حامد ضیائی تماس بگیرید"
            };
        }
    }
}
