using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.Api.Dtos;
using Contact.Api.Models;
using MongoDB.Driver;

namespace Contact.Api.Data
{
    public class MongoContactRepository : IContactRepository
    {
        private readonly ContactContext _contactContext;

        public MongoContactRepository(ContactContext contactContext)
        {
            _contactContext = contactContext;
        }

        public async Task<bool> AddContactAsync(int userId, UserIdentity contact, CancellationToken cancellationToken)
        {
            //添加前判断是否有对应的通讯录
            if(await _contactContext.ContactBooks.CountDocumentsAsync(c => c.UserId == userId) == 0)
            {
                await _contactContext.ContactBooks.InsertOneAsync(new ContactBook { UserId = userId});
            }

            var filter = Builders<ContactBook>.Filter.Eq(c => c.UserId, userId);
            var update = Builders<ContactBook>.Update.AddToSet(c => c.Contacts, new Models.Contact{
                UserId = contact.UserId,
                Avatar = contact.Avatar,
                Company = contact.Company,
                Name = contact.Name,
                Title = contact.Title
            });

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);
            return result.MatchedCount == result.MatchedCount && result.ModifiedCount == 1;
        }

        public async Task<List<Models.Contact>> GetContactListAsync(int userId, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userId,null,cancellationToken)).FirstOrDefault();
            if (contactBook != null)
            {
                return contactBook.Contacts;
            }

            //LOG TBD
            //return null;            
            return new List<Models.Contact>();
        }

        public async Task<bool> TagContanctsAsync(int userId,int contactId,List<string> tags, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.Eq(c=>c.UserId,userId),
                Builders<ContactBook>.Filter.Eq("",contactId)
                );

            var update = Builders<ContactBook>.Update
                .Set("Contact.$.Tags", tags);

            var result = await _contactContext.ContactBooks.UpdateOneAsync(filter, update, null, cancellationToken);

            return result.MatchedCount == result.ModifiedCount && result.MatchedCount == 1;
        }

        public async Task<bool> UpdateContactInfoAsync(UserIdentity userInfo, CancellationToken cancellationToken)
        {
            var contactBook = (await _contactContext.ContactBooks.FindAsync(c => c.UserId == userInfo.UserId,null,cancellationToken))
                .FirstOrDefault(cancellationToken);

            if (contactBook == null)
            {
                return true;
            }

            var contactIds = contactBook.Contacts.Select(c => c.UserId);

            var filter = Builders<ContactBook>.Filter.And(
                Builders<ContactBook>.Filter.In(c => c.UserId, contactIds),
                Builders<ContactBook>.Filter.ElemMatch(c=>c.Contacts,contact=>contact.UserId == userInfo.UserId)
                );

            var update = Builders<ContactBook>.Update
                .Set("Contacts.$.Name", userInfo.Name)
                .Set("Contacts.$.Avatar", userInfo.Avatar)
                .Set("Contacts.$.Company", userInfo.Company)
                .Set("Contacts.$.Title", userInfo.Title);

            var updateResult = await _contactContext.ContactBooks.UpdateManyAsync(filter, update,null,cancellationToken);
            return updateResult.MatchedCount == updateResult.ModifiedCount;
        }
    }
}
