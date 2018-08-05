using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contact.Api.Models;
using MongoDB.Driver;

namespace Contact.Api.Data
{
    public class MongoContactApplyRequestRepository : IContactApplyRequestRepository
    {
        private ContactContext _ContactContext;

        public MongoContactApplyRequestRepository(ContactContext ContactContext)
        {
            _ContactContext = ContactContext;
        }

        public async Task<bool> AddRequestAsync(ContactApplyRequest contactApplyRequest, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(r => r.UserId == contactApplyRequest.UserId
            && r.ApplierId == contactApplyRequest.ApplierId);
            
            if ((_ContactContext.ContactApplyRequest.CountDocuments(filter)) > 0)
            {
                var update = Builders<ContactApplyRequest>.Update
                    .Set(r => r.Approvaled,Models.Enum.ApplyStatus.Waiting)
                    .Set(r => r.HandleTime, DateTime.Now);

                var UpdateResult = await _ContactContext.ContactApplyRequest.UpdateOneAsync(filter, update, null, cancellationToken);
                return UpdateResult.MatchedCount == 1 && UpdateResult.MatchedCount == UpdateResult.ModifiedCount;
            }
            await _ContactContext.ContactApplyRequest.InsertOneAsync(contactApplyRequest, null, cancellationToken);
            return true;
        }

        public async Task<bool> ApprovalAsync(int userId,int applierId, CancellationToken cancellationToken)
        {
            var filter = Builders<ContactApplyRequest>.Filter.Where(r => r.UserId == userId
            && r.ApplierId == applierId);

            
            var update = Builders<ContactApplyRequest>.Update
                .Set(r => r.HandleTime, DateTime.Now)
                .Set(r => r.Approvaled,Models.Enum.ApplyStatus.Passed);

            var UpdateResult = await _ContactContext.ContactApplyRequest.UpdateOneAsync(filter, update, null, cancellationToken);
            return UpdateResult.MatchedCount == 1 && UpdateResult.MatchedCount == UpdateResult.ModifiedCount;
            
            
        }

        /// <summary>
        /// 获取请求列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<ContactApplyRequest>> GetRequestListAsync(int userId, CancellationToken cancellationToken)
        {
            return (await _ContactContext.ContactApplyRequest.FindAsync(r => r.UserId == userId)).ToList(cancellationToken);
            
        }
    }
}
