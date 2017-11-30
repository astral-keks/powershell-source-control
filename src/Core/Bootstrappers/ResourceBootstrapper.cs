using AstralKeks.SourceControl.Resources;
using AstralKeks.Workbench.Common.Content;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using Autofac;
using System;

namespace AstralKeks.SourceControl.Bootstrappers
{
    public class ResourceBootstrapper : IStartable
    {
        private readonly SharedContext _sharedContext;
        private readonly ResourceRepository _resourceRepository;

        public ResourceBootstrapper(SharedContext sharedContext, ResourceRepository resourceRepository)
        {
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
        }

        public void Start()
        {
            var userspaceResourcePath = PathBuilder.Combine(
                _sharedContext.CurrentUserspaceDirectory, Directories.Config, Directories.SourceControl, Files.RepositoryJson);
            _resourceRepository.CreateResource(userspaceResourcePath, Files.RepositoryJson);
        }
    }
}
