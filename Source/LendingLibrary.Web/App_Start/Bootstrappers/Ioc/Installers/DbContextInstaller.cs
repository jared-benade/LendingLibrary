using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LendingLibrary.DB;

namespace LendingLibrary.Web.Bootstrappers.Ioc.Installers
{
    public class DbContextInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILendingLibraryDbContext>()
                .ImplementedBy<LendingLibraryDbContext>()
                .LifestyleSingleton());
        }
    }
}