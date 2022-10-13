using FARMASI.Common.BaseSettings.MongoDB;
using FARMASI.DataAccessLayer.Abstract;
using FARMASI.Entity;
using FARMASI.Repository.Concrete.MongoDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.DataAccessLayer.Concrete
{
    public class ProductDAL : BaseMongoDBRepository<Product>, IProductDAL
    {
        public ProductDAL(IOptions<MongoDBSettings> mongoDBSettings)
            : base(mongoDBSettings)
        {
        }
    }
}
