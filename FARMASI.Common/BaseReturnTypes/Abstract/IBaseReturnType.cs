using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Common.BaseReturnTypes.Abstract
{
    public interface IBaseReturnType : IDisposable
    {
        public bool ErrorOccurred { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public object Data { get; set; }
    }
}
