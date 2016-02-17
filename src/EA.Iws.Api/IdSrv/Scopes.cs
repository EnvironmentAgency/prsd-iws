﻿namespace EA.Iws.Api.IdSrv
{
    using System.Collections.Generic;
    using IdentityServer3.Core.Models;

    internal static class Scopes
    {
        public static List<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                new Scope
                {
                    Name = "api1"
                }
            };

            scopes.Add(StandardScopes.AllClaims);
            scopes.Add(StandardScopes.OfflineAccess);
            scopes.AddRange(StandardScopes.All);

            return scopes;
        }
    }
}