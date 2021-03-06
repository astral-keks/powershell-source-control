﻿using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using Autofac;
using Autofac.Builder;
using Style = Autofac.Builder.SingleRegistrationStyle;
using Activator = Autofac.Builder.ConcreteReflectionActivatorData;
using AstralKeks.Workbench.Common.Template;

namespace AstralKeks.SourceControl.Bootstrappers
{
    public class ContextBootstrapper : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterUserspaceContext(builder).As<UserspaceContext>().SingleInstance();
            RegisterWorkspaceContext(builder).As<WorkspaceContext>().SingleInstance();
            RegisterGlobalContext(builder).As<GlobalContext>().SingleInstance();
            RegisterSharedContext(builder).As<SharedContext>().SingleInstance();
            RegisterTemplateProcessor(builder).As<TemplateProcessor>().SingleInstance();
            RegisterResourceRepository(builder).As<ResourceRepository>().SingleInstance();
        }

        protected virtual IRegistrationBuilder<UserspaceContext, Activator, Style> RegisterUserspaceContext(ContainerBuilder builder)
        {
            return builder.RegisterType<UserspaceContext>();
        }

        protected virtual IRegistrationBuilder<WorkspaceContext, Activator, Style> RegisterWorkspaceContext(ContainerBuilder builder)
        {
            return builder.RegisterType<WorkspaceContext>();
        }

        protected virtual IRegistrationBuilder<GlobalContext, Activator, Style> RegisterGlobalContext(ContainerBuilder builder)
        {
            return builder.RegisterType<GlobalContext>();
        }

        protected virtual IRegistrationBuilder<SharedContext, Activator, Style> RegisterSharedContext(ContainerBuilder builder)
        {
            return builder.RegisterType<SharedContext>();
        }

        protected virtual IRegistrationBuilder<TemplateProcessor, Activator, Style> RegisterTemplateProcessor(ContainerBuilder builder)
        {
            return builder.RegisterType<TemplateProcessor>();
        }

        protected virtual IRegistrationBuilder<ResourceRepository, Activator, Style> RegisterResourceRepository(ContainerBuilder builder)
        {
            return builder.RegisterType<ResourceRepository>();
        }
    }
}
