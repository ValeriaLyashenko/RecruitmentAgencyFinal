using RecruitmentAgencyFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RecruitmentAgencyFinal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /*using (var db = new AppDbContext())
            {
                if (!db.Users.Any())
                {
                    db.Users.Add(new User
                    {
                        Email = "admin@agency.ru",
                        Password = "admin123",
                        FullName = "Администратор",
                        Role = "Admin",
                        RegisteredAt = DateTime.Now,
                        IsActive = true
                    });
                    db.Users.Add(new User
                    {
                        Email = "manager@agency.ru",
                        Password = "manager123",
                        FullName = "Менеджер",
                        Role = "Manager",
                        RegisteredAt = DateTime.Now,
                        IsActive = true
                    });
                    db.SaveChanges();
                }
            }*/
    }
    }
}


