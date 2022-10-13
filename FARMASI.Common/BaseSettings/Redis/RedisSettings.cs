using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FARMASI.Common.BaseSettings.Redis
{
    public class RedisSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Db { get; set; }
    }
}
