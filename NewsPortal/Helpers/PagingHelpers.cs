using Business.Models;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NewsPortal.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for(int i = 0; i < pageInfo.TotalPages; i++)
            {
                var currentPage = i + 1;
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(currentPage));
                tag.InnerHtml = currentPage.ToString();

                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}