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

            string filterString = string.Empty;
            string searchString = string.Empty;
            SortOrder sortOrder = SortOrder.DateDescending;
            int page = 1;

            if (request.QueryString["filterString"] != null)
                filterString = request.QueryString["filterString"].ToString();

            if (request.QueryString["searchString"] != null)
                searchString = request.QueryString["searchString"].ToString();


            if (request.QueryString["sortOrder"] != null)
            {
                var sortOrderFromQuery = int.Parse(request.QueryString["sortOrder"]);
                switch (sortOrderFromQuery)
                {
                    case 1:
                        sortOrder = SortOrder.DateAscending;
                        break;
                    case 2:
                        sortOrder = SortOrder.DateDescending;
                        break;
                    case 3:
                        sortOrder = SortOrder.TitleAscending;
                        break;
                    case 4:
                        sortOrder = SortOrder.TitleDescending;
                        break;
                    case 5:
                        sortOrder = SortOrder.DescriptionAscending;
                        break;
                    case 6:
                        sortOrder = SortOrder.DescriptionDescending;
                        break;
                }
            }

            if (request.QueryString["page"] != null)
                page = int.Parse(request.QueryString["page"]);

            Criteria criteria = new Criteria
            {
                FilterString = filterString,
                SearchString = searchString,
                SortOrder = sortOrder,
                Page = page - 1,
                ArticlesPerPage = int.Parse(ConfigurationManager.AppSettings["articlesPerPage"])
            };

            return criteria;
        }
    }
}