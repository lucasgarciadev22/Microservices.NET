// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace EShopping.Infrastructure
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[] { new ApiScope("catalogapi") };

        //handle microservices APIs
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                // list of microservices
                new ApiResource("Catalog", "Catalog.API") { Scopes = { "catalogapi" } }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //machine to machine flow
                new Client
                {
                    ClientName = "Catalog API Client",
                    ClientId = "CatalogApiClient",
                    ClientSecrets = { new Secret("9b9311db-51ff-4b3b-b448-7e69ffe15009".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catalogapi" }
                }
            };
    }
}
