using RecruitmentAgencyFinal.Models;
using System;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class ResumeController : Controller
    {
        private DatabaseHelper db = new DatabaseHelper();

        // Просмотр своего резюме (для соискателя)
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserId"];
            var resume = db.GetResumeByUserId(userId);
            return View(resume);
        }

        // Создание резюме
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserId"];
            var existing = db.GetResumeByUserId(userId);

            if (existing != null)
                return RedirectToAction("Edit");

            return View(new Resume { UserId = userId, CreatedAt = DateTime.Now });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Resume resume)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                resume.CreatedAt = DateTime.Now;
                resume.Status = "Active";
                db.AddResume(resume);
                TempData["Success"] = "Резюме успешно создано!";
                return RedirectToAction("Index");
            }
            return View(resume);
        }

        // Редактирование резюме
        public ActionResult Edit()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            int userId = (int)Session["UserId"];
            var resume = db.GetResumeByUserId(userId);
            if (resume == null)
                return RedirectToAction("Create");

            return View(resume);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Resume resume)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.UpdateResume(resume);
                TempData["Success"] = "Резюме обновлено!";
                return RedirectToAction("Index");
            }
            return View(resume);
        }

        // Все резюме (только для Admin/Manager)
        public ActionResult All()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            string role = Session["UserRole"]?.ToString();
            if (role != "Admin" && role != "Manager")
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            var resumes = db.GetAllResumes();
            return View(resumes);
        }

        // Детали резюме (только для Admin/Manager)
        public ActionResult Details(int id)
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            string role = Session["UserRole"]?.ToString();
            if (role != "Admin" && role != "Manager")
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            var resume = db.GetResumeById(id);
            if (resume == null)
                return HttpNotFound();

            return View(resume);
        }
    }
}