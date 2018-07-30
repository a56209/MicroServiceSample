using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Resilience;

namespace Contact.Api.Infrastructure
{
    public class ResilientHttpClientFactory
    {
        private readonly ILogger<ResilientHttpClientFactory> _logger;
        private readonly ILogger<ResilienceHttpClient> _loggerHttpClient;
        /// <summary>
        /// 重试次数
        /// </summary>
        private readonly int _retryCount;
        /// <summary>
        /// 异常引发熔断次数
        /// </summary>
        private readonly int _exceptionsAllowedBeforeBreaking;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResilientHttpClientFactory(ILogger<ResilientHttpClientFactory> logger, 
            IHttpContextAccessor httpContextAccessor, 
            int exceptionsAllowedBeforeBreaking ,int retryCount,
            ILogger<ResilienceHttpClient> loggerHttpClient)
        {
            _logger = logger;
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _retryCount = retryCount;
            _httpContextAccessor = httpContextAccessor;
            _loggerHttpClient = loggerHttpClient;
        }

        public ResilienceHttpClient CreateResilientHttpClient()
        {
            return new ResilienceHttpClient(origin => GreatePolicies(origin), _loggerHttpClient, _httpContextAccessor);
        }

        private Policy[] GreatePolicies(string origin)
        {
            //可针对不同的Url实现不同的异常策略
            var arr = new Policy[]
            {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)),
                    (exception,timeSpan,retryCount,context) =>
                    {
                        var msg = $"第 {retryCount} 次重试 " +
                            $"of {context.PolicyKey} " +
                            $"at {context.OperationKey}, " +
                            $"due to: {exception}.";
                        _logger.LogWarning(msg);
                        _logger.LogDebug(msg);
                    }
                    ),
                Policy.Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    //启动熔断器异常的次数
                    _exceptionsAllowedBeforeBreaking,
                    //熔断时间
                    TimeSpan.FromMinutes(1),
                    (exception,duration) =>
                    {
                        // on circuit opened
                        _logger.LogTrace("熔断器打开");
                    },()=>
                    {
                        // on circuit closed
                        _logger.LogTrace("熔断器关闭");
                    })

            };
            return arr;
        }
    }
}
