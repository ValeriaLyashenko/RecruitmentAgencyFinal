using RecruitmentAgencyFinal.Models;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class VacanciesController : Controller
    {
        private DatabaseHelper db = new DatabaseHelper();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var vacancies = db.GetAllVacancies();
            return View(vacancies);
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var vacancy = db.GetVacancyById(id);
            if (vacancy == null)
                return HttpNotFound();
            return View(vacancy);
        }

        // GET: Vacancies/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            return View();
        }

        // POST: Vacancies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vacancy vacancy)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            if (ModelState.IsValid)
            {
                vacancy.CreatedAt = System.DateTime.Now;
                vacancy.CreatedBy = Session["UserEmail"]?.ToString();
                vacancy.IsActive = true;

                db.AddVacancy(vacancy);
                TempData["Success"] = "Вакансия успешно создана!";
                return RedirectToAction("Index");
            }
            return View(vacancy);
        }

        // GET: Vacancies/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            var vacancy = db.GetVacancyById(id);
            if (vacancy == null)
                return HttpNotFound();

            return View(vacancy);
        }

        // POST: Vacancies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vacancy vacancy)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            if (ModelState.IsValid)
            {
                db.UpdateVacancy(vacancy);
                TempData["Success"] = "Вакансия успешно обновлена!";
                return RedirectToAction("Index");
            }
            return View(vacancy);
        }

        // POST: Vacancies/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (Session["UserId"] == null)
                return Json(new { success = false, message = "Требуется авторизация" });

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return Json(new { success = false, message = "Доступ запрещён" });

            bool result = db.DeleteVacancy(id);
            return Json(new { success = result, message = result ? "Вакансия удалена" : "Ошибка при удалении" });
        }

        // POST: Vacancies/Activate/5
        [HttpPost]
        public JsonResult Activate(int id)
        {
            if (Session["UserId"] == null)
                return Json(new { success = false, message = "Требуется авторизация" });

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return Json(new { success = false, message = "Доступ запрещён" });

            var vacancy = db.GetVacancyById(id);
            if (vacancy != null)
            {
                vacancy.IsActive = true;
                db.UpdateVacancy(vacancy);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: Vacancies/Match/5
        public ActionResult Match(int id)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (Session["UserRole"] == null || (Session["UserRole"].ToString() != "Admin" && Session["UserRole"].ToString() != "Manager"))
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            var vacancy = db.GetVacancyById(id);
            if (vacancy == null)
                return HttpNotFound();

            ViewBag.Vacancy = vacancy;
            var matches = db.MatchCandidatesToVacancy(id);
            return View(matches);
        }
    }
}