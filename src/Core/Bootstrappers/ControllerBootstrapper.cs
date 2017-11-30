using AstralKeks.SourceControl.Controllers;
using Autofac;
using Autofac.Builder;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using Style = Autofac.Builder.SingleRegistrationStyle;

namespace AstralKeks.SourceControl.Bootstrappers
{
    public class ControllerBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterWorkingCopyController(builder).As<WorkingCopyController>().SingleInstance();
            
            base.Load(builder);
        }

        protected virtual IRegistrationBuilder<WorkingCopyController, Activator, Style> RegisterWorkingCopyController(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkingCopyController>();
        }
    }
}
