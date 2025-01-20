using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class CustomException : ApplicationException
    {
        public int ErrorCode { get; }
        public string Detail { get; }

        public CustomException(ErrorCode code)
            : base(code.GetMessage())
        {
            ErrorCode = (int)code;
            Detail = code.GetDescription();
        }
    }
}
