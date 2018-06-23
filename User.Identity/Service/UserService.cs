using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;

namespace User.Identity.Service
{
    public class UserService : IUserService
    {
        private HttpClient _httpClient;
        private readonly string _userServiceUrl = "http://localhost:51448";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
