using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity
{
    public class Config
    {
        /// <summary>
        /// 定义客户端
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client> {
                new Client
                {
                    ClientId = "pc",
                    ClientName = "pc模式",
                    //客户端授权模式,采用自定义的模式
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedGrantTypes = new List<string>{ "sms_auth_code" },

                    //允许离线，即开启refresh_token
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    //禁用consent 页面确认
                    RequireConsent = false, 
                    
                    
                    //用于认证的密码
                    ClientSecrets = {
                        new Secret("secret".Sha256())
                    },
                    //客户端有权访问的范围
                    AllowedScopes = new List<string> {
                        "gateway.api",
                        "contact.api",
                        "user.api",
                        "project.api",
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }
 
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("gateway.api","gateway api service"),
                new ApiResource("contact.api","contact api service"),
                new ApiResource("project.api","project api service"),
                new ApiResource("user.api","user api service")
            };
        }
    }
}
