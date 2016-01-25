using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using LendingLibrary.DB.Migrations;
using LendingLibrary.Web.Bootstrappers.Ioc;

namespace LendingLibrary.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ConfigureDependencyInjection();
            var connectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MigrateDatabaseWith(connectionString);
        }

        private void ConfigureDependencyInjection()
        {
            _container = CreateWindsorBootstrapper().Boostrap();
        }

        private void MigrateDatabaseWith(string connectionString)
        {
            var runner = new DBMigrationsRunner(connectionString);
            runner.MigrateToLatest();
        }

        private static WindsorBootstrapper CreateWindsorBootstrapper()
        {
            return new WindsorBootstrapper();
        }
    }
}
