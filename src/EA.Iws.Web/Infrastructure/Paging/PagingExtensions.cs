﻿/*
 * Modified from https://github.com/martijnboland/MvcPaging
 * 
 * The MIT license
 * 
 * Copyright (c) 2008-2016 Martijn Boland, Bart Lenaerts, Rajeesh CV
 * 
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */
namespace EA.Iws.Web.Infrastructure.Paging
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    public static class PagingExtensions
    {
        public static Pager Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return new Pager(htmlHelper, pageSize, currentPage, totalItemCount);
        }

        public static Pager<TModel> Pager<TModel>(this HtmlHelper<TModel> htmlHelper, int pageSize, int currentPage, int totalItemCount)
        {
            return new Pager<TModel>(htmlHelper, pageSize, currentPage, totalItemCount);
        }

        public static MvcHtmlString PagerSummary(this HtmlHelper htmlHelper, int pageSize, int currentPage,
            int totalItemCount)
        {
            var start = ((currentPage - 1) * pageSize) + 1;
            var end = Math.Min(totalItemCount, currentPage * pageSize);

            var builder = new TagBuilder("div");
            builder.AddCssClass("pager-summary");
            builder.SetInnerText(string.Format("Showing {0} &ndash; {1} of {2} results", start, end, totalItemCount));

            return MvcHtmlString.Create(HttpUtility.HtmlDecode(builder.ToString()));
        }
    }
}