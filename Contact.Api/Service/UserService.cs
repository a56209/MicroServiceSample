using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Dtos;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience;

namespace Contact.Api.Service
{
    public class UserService : IUserService
    {
        private IHttpClient _httpClient;
        private string _userServiceUrl;
        private ILogger<UserService> _logger;

        public UserService(IHttpClient httpClient, 
            IOptions<ServiceDisvoveryOptions> options,
            IDnsQuery dnsQuery, 
            ILogger<UserService> logger)
        {
            _httpClient = httpClient;            
            _logger = logger;

            var address = dnsQuery.ResolveService("service.consul", options.Value.UserServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;

            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";
        }

        public async Task<UserIdentity> GetBaseUserInfoAsync(int UserId)
        {
            _logger.LogTrace($"Enter into CheckOrCreate:{UserId}");
            

            try
            {
                string requestUrl = _userServiceUrl + "/api/users/baseinfo/" + UserId;
                var response = await _httpClient.GetStringAsync(requestUrl);

                if (! string.IsNullOrEmpty(response))
                {                                        
                    var userInfo = JsonConvert.DeserializeObject<UserIdentity>(response);

                    _logger.LogTrace($"Completed CheckOrCreate with userId:{UserId}");
                    

                    return userInfo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetBaseUserInfoAsync 在重试之后失败，" + ex.Message + ex.StackTrace);
                throw ex;
            }

            return null;
        }
    }
}
