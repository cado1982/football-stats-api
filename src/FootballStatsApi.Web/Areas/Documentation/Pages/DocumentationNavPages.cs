using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace FootballStatsApi.Web.Areas.Documentation.Pages
{
    public static class DocumentationNavPages
    {
        public static string Introduction => "Introduction";
        public static string Authentication => "Authentication";
        public static string RateLimiting => "RateLimiting";
        public static string Endpoints => "Endpoints";

        public static string IntroductionNavClass(ViewContext viewContext) => PageNavClass(viewContext, Introduction);
        public static string AuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, Authentication);
        public static string RateLimitingNavClass(ViewContext viewContext) => PageNavClass(viewContext, RateLimiting);
        public static string EndpointsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Endpoints);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
