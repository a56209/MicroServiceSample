using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Common
{
    public class AppSettings
    {
        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabase { get; set; }
    }
}
