using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Options;
using Polly;
using Resilience;
using User.Identity.Dtos;

namespace User.Identity.Service
{
    public class UserService : IUserService
    {
        //private HttpClient _httpClient;
        private IHttpClient _httpClient;
        private string _userServiceUrl;

        public UserService(IHttpClient httpClient, IOptions<ServiceDisvoveryOptions> options, IDnsQuery dnsQuery)
        {
            _httpClient = httpClient;
            var address = dnsQuery.ResolveService("service.consul", options.Value.UserServiceName);
            var addressList = address.First().AddressList;
            var host = addressList.Any() ? addressList.First().ToString() : address.First().HostName;
            var port = address.First().Port;
            _userServiceUrl = $"http://{host}:{port}";

        }

        public async Task<int> CheckOrCreatAsync(string phone)
        {
            var form = new Dictionary<string, string> { { "phone", phone } };
            var content = new FormUrlEncodedContent(form);
            string requestUrl = _userServiceUrl + "/api/users/check-or-create";
            var response = await _httpClient.PostAsync(requestUrl, content);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var userId = await response.Content.ReadAsStringAsync();
                int.TryParse(userId, out int intUserId);

                return intUserId;
            }
            return 0;
        }
    }
}
