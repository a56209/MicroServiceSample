using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Data;
using Contact.Api.IntergrationEvents;
using System.Threading;

namespace Contact.Api.IntergrationEvents.EventHandling
{
    public class UserProfileChangedEventHandler:ICapSubscribe
    {
        private IContactRepository _ContactRepository;

        public UserProfileChangedEventHandler(IContactRepository ContactRepository)
        {
            _ContactRepository = ContactRepository;
        }

        [CapSubscribe("userapi.userprofileChanged")]
        public async Task UpdateContactInfo(UserProfileChangedEvent @event)
        {
            var token = new CancellationToken();

            await _ContactRepository.UpdateContactInfoAsync(new Dtos.UserIdentity
            {
                UserId = @event.UserId,
                Name = @event.Name,
                Company = @event.Company,
                Title = @event.Title,
                Avatar = @event.Avatar
            },token);
        }
    }
}
