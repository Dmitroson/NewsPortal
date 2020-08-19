using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;

namespace NewsPortal.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            int pagingPanelSize = int.Parse(ConfigurationManager.AppSettings.Get("pagingPanelSize"));
            int currentPage = pageInfo.PageNumber + 1;
            int offset = 0;

            StringBuilder result = new StringBuilder();

            if (pageInfo.TotalPages >= pagingPanelSize)
            {
                if (currentPage > pagingPanelSize)
                {
                    offset = currentPage - pagingPanelSize;
                }
                for (int page = 1; page <= pagingPanelSize; page++)
                {
                    var p = page + offset;
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(p));
                    tag.InnerHtml = p.ToString();

                    if (p == currentPage)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-primary");
                    }
                    tag.AddCssClass("btn btn-default");
                    result.Append(tag.ToString());
                }
            }
            else
            {
                for (int i = 1; i <= pageInfo.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();

                    if (i == currentPage)
                    {
                        tag.AddCssClass("selected");
                        tag.AddCssClass("btn-primary");
                    }
                    tag.AddCssClass("btn btn-default");
                    result.Append(tag.ToString());
                }
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}