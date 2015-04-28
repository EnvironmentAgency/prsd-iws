﻿namespace EA.Iws.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Services;
    using Thinktecture.IdentityModel.Client;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app, AppConfiguration config)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = Constants.IwsAuthType,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseClaimsTransformation(incoming => TransformClaims(incoming, config));
        }

        private async Task<ClaimsPrincipal> TransformClaims(ClaimsPrincipal incoming, AppConfiguration config)
        {
            if (!incoming.Identity.IsAuthenticated)
            {
                return incoming;
            }

            // parse incoming claims - create new principal with app claims
            var claims = new List<Claim>();

            var userInfoClient = new UserInfoClient(
                new Uri(config.ApiUrl + "/connect/userinfo"),
                incoming.FindFirst(OAuth2Constants.AccessToken).Value);

            var userInfo = await userInfoClient.GetAsync();
            userInfo.Claims.ToList().ForEach(ui => claims.Add(new Claim(ui.Item1, ui.Item2)));

            // keep the id_token for logout
            var idToken = incoming.FindFirst(OAuth2Constants.IdentityToken);
            if (idToken != null)
            {
                claims.Add(new Claim(OAuth2Constants.IdentityToken, idToken.Value));
            }

            // add access token for sample API
            claims.Add(incoming.FindFirst(OAuth2Constants.AccessToken));

            // keep track of access token expiration
            claims.Add(incoming.FindFirst(IwsClaimTypes.ExpiresAt));

            var nameId = incoming.FindFirst(ClaimTypes.NameIdentifier);
            if (nameId != null)
            {
                claims.Add(nameId);
            }

            var thumbprint = incoming.FindFirst(ClaimTypes.Thumbprint);
            if (thumbprint != null)
            {
                claims.Add(thumbprint);
            }

            var id = new ClaimsIdentity(Constants.IwsAuthType);
            id.AddClaims(claims);

            return new ClaimsPrincipal(id);
        }
    }
}