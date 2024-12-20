﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
            new ApiScope[]
            {
                //catalog operations
                new ApiScope("catalogapi"),
                new ApiScope("catalogapi.read"),
                new ApiScope("catalogapi.write"),
                //basket operations
                new ApiScope("basketapi"),
                //ocelot api gateway operations
                new ApiScope("eshoppinggateway"),
            };

        //handle microservices APIs
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                // list of microservices scopes
                new ApiResource("Catalog", "Catalog.API")
                {
                    Scopes = { "catalogapi.read", "catalogapi.write" }
                },
                new ApiResource("Basket", "Basket.API") { Scopes = { "basketapi" } },
                new ApiResource("EShoppingGateway", "EShopping Gateway")
                {
                    Scopes = { "eshoppinggateway", "basketapi" }
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                //machine to machine flow for each client
                new Client
                {
                    ClientName = "Catalog API Client",
                    ClientId = "CatalogApiClient",
                    ClientSecrets = { new Secret("9b9311db-51ff-4b3b-b448-7e69ffe15009".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catalogapi.read", "catalogapi.write" }
                },
                new Client
                {
                    ClientName = "Basket API Client",
                    ClientId = "BasketApiClient",
                    ClientSecrets = { new Secret("7dbefbf0-d48d-4702-8ba0-ea40b34e06f3".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "basketapi" }
                },
                new Client
                {
                    ClientName = "EShopping Gateway Client",
                    ClientId = "EShoppingGatewayClient",
                    ClientSecrets = { new Secret("5c7fd5c5-61a7-4668-ac57-2b4591ec26d2".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "eshoppinggateway", "basketapi" }
                },
            };
    }
}
