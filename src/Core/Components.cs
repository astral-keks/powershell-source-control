using AstralKeks.SourceControl.Bootstrappers;
using AstralKeks.SourceControl.Controllers;
using AstralKeks.SourceControl.Managers;
using Autofac;

namespace AstralKeks.SourceControl
{
    public class ComponentContainer
    {
        private readonly IContainer _container;

        public ComponentContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InfrastructureBootstrapper());
            builder.RegisterModule(new ContextBootstrapper());
            builder.RegisterModule(new ManagerBootstrapper());
            builder.RegisterModule(new ControllerBootstrapper());
            builder.RegisterType<ResourceBootstrapper>().As<IStartable>();
            _container = builder.Build();
        }

        public RepositoryManager RepositoryManager => _container.Resolve<RepositoryManager>();
        public WorkingCopyManager WorkingCopyManager => _container.Resolve<WorkingCopyManager>();
        public WorkingCopyController WorkingCopyController => _container.Resolve<WorkingCopyController>();
    }
}
