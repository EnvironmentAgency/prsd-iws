﻿namespace EA.Iws.Api.Filters
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class MaintenanceModeFilter : IActionFilter
    {
        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        } 

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            return Task.FromResult(actionContext.Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, "Maintenance"));
        }
    }
}