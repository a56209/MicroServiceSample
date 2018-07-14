using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilience
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync<T>(string url, T item, string authorizationToken=null, string requestId = null, string authorizationMethod = "Bearer");
        Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> dictionary, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer");

    }
}
