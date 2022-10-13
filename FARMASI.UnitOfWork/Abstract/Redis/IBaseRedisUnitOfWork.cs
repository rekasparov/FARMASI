using FARMASI.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.UnitOfWork.Abstract.Redis
{
    public interface IBaseRedisUnitOfWork
    {
        public IShoppingCartDAL ShoppingCart { get; }
    }
}
