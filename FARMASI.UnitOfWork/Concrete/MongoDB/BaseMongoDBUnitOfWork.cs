using FARMASI.Common.BaseSettings.MongoDB;
using FARMASI.DataAccessLayer.Abstract;
using FARMASI.DataAccessLayer.Concrete;
using FARMASI.UnitOfWork.Abstract.MongoDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.UnitOfWork.Concrete.MongoDB
{
    public class BaseMongoDBUnitOfWork : IBaseMongoDBUnitOfWork
    {
        private readonly IOptions<MongoDBSettings> mongoDBSettings;

        public BaseMongoDBUnitOfWork(IOptions<MongoDBSettings> mongoDBSettings)
        {
            this.mongoDBSettings = mongoDBSettings;
        }

        public IProductDAL Product => new ProductDAL(mongoDBSettings);
    }
}
