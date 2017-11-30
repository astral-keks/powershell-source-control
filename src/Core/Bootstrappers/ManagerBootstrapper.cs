using AstralKeks.SourceControl.Managers;
using Autofac;
using Autofac.Builder;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using Style = Autofac.Builder.SingleRegistrationStyle;

namespace AstralKeks.SourceControl.Bootstrappers
{
    public class ManagerBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterRepositoryManager(builder).As<RepositoryManager>().SingleInstance();
            RegisterWorkingCopyManager(builder).As<WorkingCopyManager>().SingleInstance();

            base.Load(builder);
        }

        protected virtual IRegistrationBuilder<RepositoryManager, Activator, Style> RegisterRepositoryManager(ContainerBuilder builder)
        {
            return builder.RegisterType<RepositoryManager>();
        }

        protected virtual IRegistrationBuilder<WorkingCopyManager, Activator, Style> RegisterWorkingCopyManager(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkingCopyManager>();
        }
    }
}
