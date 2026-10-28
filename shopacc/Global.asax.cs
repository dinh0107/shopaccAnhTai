using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using shopacc.DAL;
using shopacc.Migrations;
using shopacc.Models;

namespace shopacc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataEntities, Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            using (var db = new DataEntities())
            {
                try
                {
                    db.Database.ExecuteSqlCommand(@"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ConfigSites' AND COLUMN_NAME = 'ThemeEvent')
                        BEGIN
                            ALTER TABLE ConfigSites ADD ThemeEvent NVARCHAR(50) NULL;
                        END
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ConfigSites' AND COLUMN_NAME = 'EnableThemeEffect')
                        BEGIN
                            ALTER TABLE ConfigSites ADD EnableThemeEffect BIT NOT NULL DEFAULT 1;
                        END
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ConfigSites' AND COLUMN_NAME = 'ThemeGreeting')
                        BEGIN
                            ALTER TABLE ConfigSites ADD ThemeGreeting NVARCHAR(200) NULL;
                        END
                    ");
                }
                catch { }
            }

            using (var unitofWork = new UnitOfWork())
            {
                var cfg = unitofWork.ConfigSiteRepository.GetQuery().FirstOrDefault();
                if (cfg == null)
                {
                    cfg = new ConfigSite
                    {
                        Title = "Shop Acc Anh Tài",
                        Description = "Shop bán acc game uy tín hàng đầu",
                        Name = "Anh Tài",
                        Hotline = "0987654321",
                        ThemeEvent = "none",
                        EnableThemeEffect = true,
                        ThemeGreeting = "Chào mừng quý khách đến với Shop Acc Anh Tài!"
                    };
                    unitofWork.ConfigSiteRepository.Insert(cfg);
                    unitofWork.Save();
                }
                Application["ConfigSite"] = cfg;

                if (!unitofWork.AdminRepository.GetQuery(a => a.Username == "admin").Any())
                {
                    var pass = Helpers.HtmlHelpers.ComputeHash("admin123", "SHA256", null);
                    unitofWork.AdminRepository.Insert(new Admin { Username = "admin", Password = pass, Active = true });
                    unitofWork.Save();
                }
            }
        }
    }
}
