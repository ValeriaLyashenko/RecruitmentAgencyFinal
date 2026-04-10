using RecruitmentAgencyFinal.Models;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseHelper db = new DatabaseHelper();

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.TotalVacancies = db.GetAllVacancies().Count;
            ViewBag.ActiveVacancies = db.GetAllVacancies().FindAll(v => v.IsActive).Count;
            ViewBag.TotalCandidates = db.GetAllCandidates().Count;

            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}