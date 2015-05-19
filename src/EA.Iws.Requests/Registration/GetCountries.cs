﻿namespace EA.Iws.Requests.Registration
{
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetCountries : IRequest<CountryData[]>
    {
    }
}