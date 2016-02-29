using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LendingLibrary.Core.Interfaces.Repositories;
using LendingLibrary.DB.Repositories;

namespace LendingLibrary.Web.Bootstrappers.Ioc.Installers
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPersonRepository>()
                .ImplementedBy<PersonRepository>()
                .LifestylePerWebRequest());

            container.Register(Component.For<IItemRepository>()
                .ImplementedBy<ItemRepository>()
                .LifestylePerWebRequest());

            container.Register(Component.For<IItemTypeRepository>()
                .ImplementedBy<ItemTypeRepository>()
                .LifestylePerWebRequest());

            container.Register(Component.For<ILendingTransactionRepository>()
                .ImplementedBy<LendingTransactionRepository>()
                .LifestylePerWebRequest());
        }
    }
}