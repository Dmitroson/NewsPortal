using Business.Models;
using System.Configuration;
using System.Web.Mvc;

namespace NewsPortal.CustomModelBinders
{
    public class CriteriaModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;

            DateRange range = new DateRange();
            string searchString = string.Empty;
            SortOrder sortOrder = SortOrder.DateDescending;
            int page = 1;

            if (request.QueryString["filterString"] != null)
            {
                var filterString = request.QueryString["filterString"].ToString();
                range = new DateRange(filterString);
            }

            if (request.QueryString["searchString"] != null)
                searchString = request.QueryString["searchString"].ToString();


            if (request.QueryString["sortOrder"] != null)
            {
                var sortOrderFromQuery = request.QueryString["sortOrder"];
                sortOrder = GetSortOrderFromQuery(sortOrderFromQuery);
            }

            if (request.QueryString["page"] != null)
                page = int.Parse(request.QueryString["page"]);

            Criteria criteria = new Criteria
            {
                FilterRange = range,
                SearchString = searchString,
                SortOrder = sortOrder,
                Page = page - 1,
                ArticlesPerPage = int.Parse(ConfigurationManager.AppSettings["articlesPerPage"])
            };

            return criteria;
        }

        private SortOrder GetSortOrderFromQuery(string sortOrderFromQuery)
        {
            switch (sortOrderFromQuery)
            {
                case "date":
                    return SortOrder.DateAscending;
                case "dateDescending":
                    return SortOrder.DateDescending;
                case "title":
                    return SortOrder.TitleAscending;
                case "titleDescending":
                    return SortOrder.TitleDescending;
                case "description":
                    return SortOrder.DescriptionAscending;
                case "descriptionDescending":
                    return SortOrder.DescriptionDescending;

                default: return SortOrder.DateDescending;
            }
        }
    }
}