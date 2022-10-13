using FARMASI.Common.BaseEntities.Abstract;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Common.BaseEntities.Concrete
{
    public class BaseEntity : IBaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    }
}
