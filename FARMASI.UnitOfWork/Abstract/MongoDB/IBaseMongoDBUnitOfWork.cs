using FARMASI.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.UnitOfWork.Abstract.MongoDB
{
    public interface IBaseMongoDBUnitOfWork
    {
        public IProductDAL Product { get; }
    }
}
