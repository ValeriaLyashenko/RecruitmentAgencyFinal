using RecruitmentAgencyFinal.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class UsersController : Controller
    {
        // Временное хранилище пользователей (в памяти)
        private static List<UserTemp> _users = AccountController.GetUsers();

        // Только для Admin (проверка вручную)
        public ActionResult Index()
        {
            // Проверяем, авторизован ли пользователь и имеет ли роль Admin
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return new HttpStatusCodeResult(403, "Доступ запрещён. Требуется роль Администратора.");
            }

            return View(_users);
        }

        [HttpPost]
        public JsonResult EditRole(int id, string role)
        {
            if (Session["UserId"] == null || Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return Json(new { success = false, message = "Доступ запрещён" });
            }

            var user = _users.Find(u => u.Id == id);
            if (user != null)
            {
                user.Role = role;
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult ToggleActive(int id)
        {
            if (Session["UserId"] == null || Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return Json(new { success = false, message = "Доступ запрещён" });
            }

            var user = _users.Find(u => u.Id == id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                return Json(new { success = true, isActive = user.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult SetAccess(int id, int days)
        {
            if (Session["UserId"] == null || Session["UserRole"] == null || Session["UserRole"].ToString() != "Admin")
            {
                return Json(new { success = false, message = "Доступ запрещён" });
            }

            var user = _users.Find(u => u.Id == id);
            if (user != null)
            {
                user.AccessUntil = days > 0 ? System.DateTime.Now.AddDays(days) : (System.DateTime?)null;
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}