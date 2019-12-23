// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                     new List<string>() { "role" })
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[] 
            {
                 new ApiResource(
                    name: "usermanager_api",
                    displayName: "Uer manager core api",
                    claimTypes: new List<string>() { "role", "email" })
            };
        
        public static IEnumerable<Client> Clients =>
            new Client[] 
            {
                new Client
                {
                    ClientId = "js_oidc",
                    ClientName = "JavaScript OIDC Client",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AccessTokenLifetime = 180,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris =new List<string>
                    {
                        "http://localhost:4200/signin-callback",
                        "http://localhost:4200/silent-callback.html"
                    },

                    PostLogoutRedirectUris = { "http://localhost:4200/postlogout-callback" },
                    AllowedCorsOrigins = { "http://localhost:4200" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "roles",
                        "usermanager_api"
                    }
                }
            };
        
    }
}