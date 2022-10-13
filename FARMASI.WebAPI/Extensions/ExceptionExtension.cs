using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI.Extensions
{
    public static class ExceptionExtension
    {
        public static Exception GetInnerException(this Exception exception)
        {
            if (exception.InnerException == null)
                return exception;
            else
            {
                exception = exception.InnerException;

                return GetInnerException(exception);
            }
        }
    }
}
