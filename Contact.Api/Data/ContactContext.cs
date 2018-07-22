using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Common;
using Contact.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB;
using MongoDB.Driver;

namespace Contact.Api.Data
{
    public class ContactContext
    {
        private IMongoDatabase _mongoDatabase;
        private IMongoCollection<ContactBook> _collection;
        private AppSettings _appSettings;

        public ContactContext(IOptionsSnapshot<AppSettings> settings)
        {
            _appSettings = settings.Value;
            var client = new MongoClient(_appSettings.MongoDbConnectionString);
            if(client != null)
            {
                _mongoDatabase = client.GetDatabase(_appSettings.MongoDbDatabase);
            }
        }

        private void CheckAndCreateCollection(string collectionName)
        {
            var collectionList = _mongoDatabase.ListCollections().ToList();
            var collectionNames = new List<string>();

            collectionList.ForEach(b => collectionNames.Add(b["name"].AsString));
            if(!collectionNames.Contains(collectionName))
            {
                _mongoDatabase.CreateCollection(collectionName);
            }
        }


        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContactBook> ContactBooks
        { get
            {
                CheckAndCreateCollection("ContactBooks");
                return _mongoDatabase.GetCollection<ContactBook>("ContactBooks");
            }
        }

        /// <summary>
        /// 好友申请请求记录
        /// </summary>
        public IMongoCollection<ContactApplyRequest> ContactApplyRequest {
            get
            {
                CheckAndCreateCollection("ContactApplyRequest");
                return _mongoDatabase.GetCollection<ContactApplyRequest>("ContactApplyRequest");
            }
        }
    }
}
