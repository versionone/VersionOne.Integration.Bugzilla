using Ninject;

namespace VersionOne.ServiceHost.Core {
    public class ModeBase {
        private readonly IKernel container;
        protected readonly CommonMode Starter;

        protected ModeBase() {
            container = new StandardKernel();
            Starter = CommonModeFactory.CreateStartup(container);

            container.Bind<IDependencyInjector>().ToConstant(new DependencyInjector(container)).InSingletonScope();
        }
    }
}