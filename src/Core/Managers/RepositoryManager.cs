using System;
using System.Collections.Generic;
using System.Linq;
using AstralKeks.SourceControl.Models;
using AstralKeks.Workbench.Common.Content;
using AstralKeks.SourceControl.Clients;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.SourceControl.Resources;
using AstralKeks.Workbench.Common.Template;
using AstralKeks.Workbench.Common.Infrastructure;

namespace AstralKeks.SourceControl.Managers
{
    public class RepositoryManager
    {
        private readonly SharedContext _sharedContext;
        private readonly TemplateProcessor _templateProcessor;
        private readonly ResourceRepository _resourceRepository;
        private readonly RepositoryClient[] _repositoryClients;
        
        public RepositoryManager(SharedContext sharedContext, TemplateProcessor templateProcessor,
            ResourceRepository resourceRepository, ProcessLauncher processLauncher)
        {
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
            _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _resourceRepository = resourceRepository ?? throw new ArgumentNullException(nameof(resourceRepository));
            
            _repositoryClients = new RepositoryClient[]
            {
                new SvnRepositoryClient(processLauncher ?? throw new ArgumentNullException(nameof(processLauncher))),
                new GitRepositoryClient(processLauncher ?? throw new ArgumentNullException(nameof(processLauncher))),
            };
        }

        public IEnumerable<Repository> GetRepositories(string name = null)
        {
            var workspaceResourcePath = PathBuilder.Complete(
                _sharedContext.CurrentWorkspaceDirectory, Directories.Config, Directories.SourceControl, Files.RepositoryJson);
            var userspaceResourcePath = PathBuilder.Combine(
                _sharedContext.CurrentUserspaceDirectory, Directories.Config, Directories.SourceControl, Files.RepositoryJson);
            var workspaceResource = _templateProcessor.Transform(_resourceRepository.GetResource(workspaceResourcePath));
            var userspaceResource = _templateProcessor.Transform(_resourceRepository.GetResource(userspaceResourcePath));

            var repositoryConfig = userspaceResource?.Read<Repository[]>(workspaceResource) ?? new Repository[0];
            var repositories = repositoryConfig.Where(r => name == null || r.Name == name).ToList();
            if (name != null && !repositories.Any())
                throw new ArgumentException("Repository was not found");
            return repositories;
        }

        public RepositoryClient GetRepositoryClient(VersionSystem vcs)
        {
            return _repositoryClients.FirstOrDefault(c => c.Vcs == vcs);
        }

        public RepositoryClient GetRepositoryClient(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
                throw new ArgumentNullException(nameof(directory));

            return _repositoryClients.FirstOrDefault(c => c.IsWorkingCopy(directory));
        }
    }
}
