using RecruitmentAgencyFinal.Models;
using System;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class ApplicationsController : Controller
    {
        private DatabaseHelper db = new DatabaseHelper();

        // Откликнуться на вакансию
        [HttpPost]
        public JsonResult Apply(int vacancyId)
        {
            if (Session["UserId"] == null)
                return Json(new { success = false, message = "Требуется авторизация" });

            int userId = (int)Session["UserId"];
            var resume = db.GetResumeByUserId(userId);

            if (resume == null)
                return Json(new { success = false, message = "Сначала заполните резюме в разделе 'Моё резюме'" });

            if (db.HasApplied(resume.Id, vacancyId))
                return Json(new { success = false, message = "Вы уже откликались на эту вакансию" });

            var application = new Application
            {
                ResumeId = resume.Id,
                VacancyId = vacancyId,
                AppliedAt = DateTime.Now,
                Status = "Pending"
            };

            bool result = db.AddApplication(application);
            if (result)
                return Json(new { success = true, message = "Отклик успешно отправлен!" });
            else
                return Json(new { success = false, message = "Ошибка при отправке отклика" });
        }

        // Просмотр всех откликов (только для Admin/Manager)
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
                return RedirectToAction("Login", "Account");

            string role = Session["UserRole"]?.ToString();
            if (role != "Admin" && role != "Manager")
                return new HttpStatusCodeResult(403, "Доступ запрещён");

            var applications = db.GetAllApplications();
            return View(applications);
        }

        // Изменить статус отклика
        [HttpPost]
        public JsonResult UpdateStatus(int id, string status, string comment)
        {
            if (Session["UserId"] == null)
                return Json(new { success = false });

            string role = Session["UserRole"]?.ToString();
            if (role != "Admin" && role != "Manager")
                return Json(new { success = false });

            db.UpdateApplicationStatus(id, status, comment);
            return Json(new { success = true });
        }
    }
}