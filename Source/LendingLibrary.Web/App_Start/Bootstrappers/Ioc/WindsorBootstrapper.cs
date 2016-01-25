using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace LendingLibrary.Web.Bootstrappers.Ioc
{
    public interface IWindsorBootstrapper
    {
        IWindsorContainer Boostrap();
    }

    public class WindsorBootstrapper : IWindsorBootstrapper
    {

        public IWindsorContainer Boostrap()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            container.Register(Component.For<IWindsorContainer>()
                .Instance(container));

            SetControllerFactory(container);

            return container;
        }

        private static void SetControllerFactory(WindsorContainer container)
        {
            var windsorControllerFactory = container.Resolve<IControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(windsorControllerFactory);
        }
    }
}