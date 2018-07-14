using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Wrap;
using Newtonsoft;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;

namespace Resilience
{
    public class ResilienceHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;
        //根据Url Origin去创建Policy
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;
        //把Policy打包组合成policy wraper,进行本地缓存
        private readonly ConcurrentDictionary<string, PolicyWrap> _policyWrappers;
        private readonly ILogger<ResilienceHttpClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilienceHttpClient(Func<string, IEnumerable<Policy>> _policyCreatort,
            ILogger<ResilienceHttpClient> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _policyWrappers = new ConcurrentDictionary<string, PolicyWrap>();
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = CreateHttpRequestMessage<T>(HttpMethod.Post, url, item);
            return await DoPostPutAsync(HttpMethod.Post, url, requestMessage, authorizationToken, requestId, authorizationMethod);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string,string> form, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var requestMessage = CreateHttpRequestMessage(HttpMethod.Post, url, form);
            return await DoPostPutAsync(HttpMethod.Post, url, requestMessage, authorizationToken, requestId, authorizationMethod);
        }

        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpMethod method,string url,T item)
        {
            return new HttpRequestMessage(method, url) {
                Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json")
            };
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url, Dictionary<string, string> form)
        {
            var requestMessage = new HttpRequestMessage(method, url);
            requestMessage.Content = new FormUrlEncodedContent(form);
            return requestMessage;
        }

        private Task<HttpResponseMessage> DoPostPutAsync(HttpMethod method, string url, HttpRequestMessage requestMessage, string authorizationToken, string requestId = null, string authorizationMethod = "Bearer")
        {
            if( method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            var origin = GetOriginFormUri(url);
            return HttpInvoker(origin, async (Context) =>
            {
                //var requestMessage = new HttpRequestMessage(method, origin);

                SetAuthorizationHeader(requestMessage);

                //requestMessage.Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json");

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);

                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }

                return response;
            });

        }

        private async Task<T> HttpInvoker<T>(string origin, Func<Context, Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);

            if (_policyWrappers.TryGetValue(normalizedOrigin,out PolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(_policyCreator(normalizedOrigin).ToArray());
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }

            //return await policyWrap.ExecuteAsync(action, new Context(normalizedOrigin));
            return await policyWrap.ExecuteAsync(action, new Context(normalizedOrigin));
        }


        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();                    
        }

        private static string GetOriginFormUri(string uri)
        {
            var url = new Uri(uri);

            var origin = $"{url.Scheme}:{url.DnsSafeHost}:{url.Port}";

            return origin;
        }

        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {            
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (! string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }

        
    }
}
