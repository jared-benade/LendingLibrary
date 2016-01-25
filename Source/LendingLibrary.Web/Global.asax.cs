using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LendingLibrary.DB.Migrations;

namespace LendingLibrary.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var connectionString = "";
            MigrateDatabaseWith(connectionString);
        }

        private void MigrateDatabaseWith(string connectionString)
        {
            var runner = new DBMigrationsRunner(connectionString);
            runner.MigrateToLatest();
        }

    }
}
