using FARMASI.Entity;
using FARMASI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.DataAccessLayer.Abstract
{
    public interface IShoppingCartDAL : IBaseRepository<ShoppingCart, string>
    {
    }
}
