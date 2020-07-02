using NewsPortal.Models;
using NHibernate;
using System.Linq;
using System.Web.Mvc;

namespace NewsPortal.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var articles = session.Query<Article>().ToList();
                return View(articles);
            }
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
    }
}