using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Contact.Api.Common;
using Contact.Api.Data;
using Contact.Api.Dtos;
using Contact.Api.Infrastructure;
using Contact.Api.IntergrationEvents.EventHandling;
using Contact.Api.Service;
using DnsClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience;

namespace Contact.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(x =>
            {
                x.MongoDbConnectionString = Configuration["MongoContactConnectionString"].ToString();
                x.MongoDbDatabase = Configuration["MongoContactDatabase"].ToString();
            });

            services.AddSingleton<ContactContext>();

            services.AddScoped<IContactApplyRequestRepository, MongoContactApplyRequestRepository>();

            services.AddScoped<IContactRepository, MongoContactRepository>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<UserProfileChangedEventHandler>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = "contact.api";
                    //最终要填写网关地址，由网关转发
                    options.Authority = "http://localhost:62352";
                    options.SaveToken = true;
                });

            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

            services.AddSingleton<IDnsQuery>(p =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfiguration.Consul.DnsEndpoint.ToIPEndPoint());
            });
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;

                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));


            //注册全局单例ResilientHttpClientFactory
            services.AddSingleton(typeof(ResilientHttpClientFactory), sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ResilientHttpClientFactory>>();
                var loggerHttpClient = sp.GetRequiredService<ILogger<ResilienceHttpClient>>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var retryCount = 6;
                var exceptionsAllowedBeforeBreaking = 5;
                return new ResilientHttpClientFactory(logger, httpContextAccessor, exceptionsAllowedBeforeBreaking, retryCount, loggerHttpClient);

            });

            //注册全局单例IhttpClient
            services.AddSingleton<IHttpClient>(sp =>
            {
                return sp.GetRequiredService<ResilientHttpClientFactory>().CreateResilientHttpClient();
            });
            

            services.AddMvc();

            services.AddCap(x =>
            {
                x.UseMySql(Configuration.GetConnectionString("MysqlUser"));
                x.UseRabbitMQ("111.231.243.162");

                //Register Dashborad
                x.UseDashboard();

                x.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = "localhost";
                    d.DiscoveryServerPort = 8500;
                    d.CurrentNodeHostName = "localhost";
                    d.CurrentNodePort = 5801;
                    d.NodeId = 2;
                    d.NodeName = "CAP No.2 Node";

                });
            }               
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();            
            app.UseMvc();
        }
    }
}
