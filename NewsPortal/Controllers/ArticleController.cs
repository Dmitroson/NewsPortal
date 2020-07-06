using NewsPortal.Models;
using NHibernate;
using System.Linq;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index(string sortOrder = "Date")
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>();
                switch (sortOrder)
                {
                    case "Title":
                        articles = articles.OrderBy(a => a.Title);
                        break;
                    case "Description":
                        articles = articles.OrderBy(a => a.Description);
                        break;
                    default:
                        articles = articles.OrderBy(a => a.PubDate);
                        break;
                }
                return View(articles.ToList());
            }
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


    }
}