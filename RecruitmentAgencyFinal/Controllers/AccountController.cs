using RecruitmentAgencyFinal.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RecruitmentAgencyFinal.Controllers
{
    public class AccountController : Controller
    {
        // Временное хранилище пользователей (в памяти)
        private static List<UserTemp> _users = new List<UserTemp>();

        static AccountController()
        {
            // Добавляем тестовых пользователей
            _users.Add(new UserTemp { Id = 1, Email = "admin@agency.ru", Password = "admin123", FullName = "Администратор", Role = "Admin", RegisteredAt = DateTime.Now, IsActive = true, AccessUntil = null });
            _users.Add(new UserTemp { Id = 2, Email = "manager@agency.ru", Password = "manager123", FullName = "Менеджер", Role = "Manager", RegisteredAt = DateTime.Now, IsActive = true, AccessUntil = null });
            _users.Add(new UserTemp { Id = 3, Email = "user@mail.ru", Password = "123", FullName = "Обычный пользователь", Role = "Applicant", RegisteredAt = DateTime.Now, IsActive = true, AccessUntil = null });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string returnUrl)
        {
            var user = _users.Find(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Проверка разового доступа
                if (user.AccessUntil.HasValue && user.AccessUntil.Value < DateTime.Now)
                {
                    ViewBag.Error = "Ваш срок доступа истёк. Обратитесь к администратору.";
                    return View();
                }

                Session["UserId"] = user.Id;
                Session["UserRole"] = user.Role;
                Session["UserEmail"] = user.Email;
                Session["UserFullName"] = user.FullName;

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Неверный email или пароль";
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string fullName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(fullName))
            {
                ViewBag.Error = "Все поля обязательны для заполнения!";
                return View();
            }

            if (_users.Exists(u => u.Email == email))
            {
                ViewBag.Error = "Пользователь с таким email уже существует!";
                return View();
            }

            var newUser = new UserTemp
            {
                Id = _users.Count + 1,
                Email = email,
                Password = password,
                FullName = fullName,
                Role = "Applicant",
                RegisteredAt = DateTime.Now,
                IsActive = true,
                AccessUntil = null
            };
            _users.Add(newUser);

            Session["UserId"] = newUser.Id;
            Session["UserRole"] = newUser.Role;
            Session["UserEmail"] = newUser.Email;
            Session["UserFullName"] = newUser.FullName;

            return RedirectToAction("Index", "Home");
        }
        public static List<UserTemp> GetUsers()
        {
            return _users;
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}