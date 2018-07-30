using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using System.Collections.Concurrent;
using Polly.Wrap;
using Microsoft.Extensions.Logging;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Resilience
{
    public  class ResilienceHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;
        //polly根据origin去创建policy
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;

        private ConcurrentDictionary<string, PolicyWrap> _policyWrappers;

        private readonly ILogger<ResilienceHttpClient> _logger;
        //拿到当前httpclient
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResilienceHttpClient(Func<string, IEnumerable<Policy>> policyCreator,  ILogger<ResilienceHttpClient> logger,IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _policyCreator = policyCreator;                     
            _policyWrappers = new ConcurrentDictionary<string, PolicyWrap>();
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<HttpResponseMessage>  PostAsync<T>(string url, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpMessage(HttpMethod.Post, url, item);

            return await DoPostAsync(HttpMethod.Post, url, func, authorizationToken, requestId, authorizationMethod);
        }
        public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> form, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpMessage(HttpMethod.Post, url, form);

            return await DoPostAsync(HttpMethod.Post, url, func, authorizationToken, requestId, authorizationMethod);
        }
        private HttpRequestMessage CreateHttpMessage<T>(HttpMethod httpMethod,string url,T item)
        {
            return new HttpRequestMessage(httpMethod, url) { Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json") };
        }
        private HttpRequestMessage CreateHttpMessage(HttpMethod httpMethod, string url, Dictionary<string,string> form)
        {
            return new HttpRequestMessage(httpMethod, url) { Content = new FormUrlEncodedContent(form) };
        }

        private  Task<HttpResponseMessage> DoPostAsync<T>(HttpMethod method,string url, Func<HttpRequestMessage> requestMessageAction, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            if (method != HttpMethod.Post && method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            // a new StringContent must be created for each retry 
            // as it is disposed after each call
            var origin = GetOriginFromUri(url);

            return HttpInvoker(origin, async (context) =>
            {
              
               
                var requestMessage = requestMessageAction();
             
                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                var response = await _httpClient.SendAsync(requestMessage);

                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }

                return response;
            });
        }
        private async Task<T> HttpInvoker<T>(string origin, Func<Context,Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);
        
            if (!_policyWrappers.TryGetValue(normalizedOrigin, out PolicyWrap policyWrap))
            {
                policyWrap = Policy.WrapAsync(_policyCreator(normalizedOrigin).ToArray());
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }

            // Executes the action applying all 
            // the policies defined in the wrapper
            return await policyWrap.ExecuteAsync(action,new Context(normalizedOrigin));
        }


        /// <summary>
        /// 把url转换成小写
        /// </summary>
        /// <param name="origin">传入的url</param>
        /// <returns></returns>
        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }

        /// <summary>
        /// 解析string类型的url
        /// </summary>
        /// <param name="uri">传入的url</param>
        /// <returns>可访问的地址</returns>
        private static string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);

            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";

            return origin;
        }
        /// <summary>
        /// 把htt请求头放到http寄存器中
        /// </summary>
        /// <param name="requestMessage"></param>
        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                requestMessage.Headers.Add("Authorization", new List<string>() { authorizationHeader });
            }
        }

        public Task<string> GetStringAsync(string url, string authorizationToken = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(url);

            return HttpInvoker(origin, async (Context) =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                var response = await _httpClient.SendAsync(requestMessage);

                // raise exception if HttpResponseCode 500 
                // needed for circuit breaker to track fails

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadAsStringAsync();
            });
        }

        public Task<HttpResponseMessage> DeleteAsync(string url, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            var origin = GetOriginFromUri(url);

            return HttpInvoker(origin, async (Context) =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);

                SetAuthorizationHeader(requestMessage);

                if (authorizationToken != null)
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
                }

                if (requestId != null)
                {
                    requestMessage.Headers.Add("x-requestid", requestId);
                }

                return await _httpClient.SendAsync(requestMessage);
            });
        }

        public Task<HttpResponseMessage> PutAsync<T>(string url, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer")
        {
            Func<HttpRequestMessage> func = () => CreateHttpMessage(HttpMethod.Post, url, item);
            return DoPostAsync(HttpMethod.Put, url, func, authorizationToken, requestId, authorizationMethod);
        }
    }
}
