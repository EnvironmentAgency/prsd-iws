﻿namespace EA.Iws.Requests.Admin
{
    using System.Collections.Generic;
    using Core.Admin.Search;
    using Prsd.Core.Mediator;

    public class GetBasicSearchResults : IRequest<IList<BasicSearchResult>>
    {
        public string SearchTerm { get; set; }

        public GetBasicSearchResults(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}