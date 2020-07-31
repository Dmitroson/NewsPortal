using Business.Models;
using System.Web.Mvc;

namespace NewsPortal.CustomModelBinders
{
    public class CriteriaModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;

            string filterString = "";
            string searchString = "";
            int sortOrder = 2;
            int page = 1;

            if (request.QueryString["filterString"] != null)
                filterString = request.QueryString["filterString"].ToString();

            if (request.QueryString["searchString"] != null)
                searchString = request.QueryString["searchString"].ToString();

            if (request.QueryString["sortOrder"] != null)
                sortOrder = int.Parse(request.QueryString["sortOrder"]);

            if (request.QueryString["page"] != null)
                page = int.Parse(request.QueryString["page"]);

            Criteria criteria = new Criteria
            {
                FilterString = filterString,
                SearchString = searchString,
                SortOrder = sortOrder,
                Page = page - 1
            };

            return criteria;
        }
    }
}