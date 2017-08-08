using System;
using System.Collections.Generic;
using System.Linq;
using AstralKeks.SourceControl.Core.Client;
using AstralKeks.SourceControl.Core.Data;

namespace AstralKeks.SourceControl.Core.Management
{
    public class RepositoryManager
    {
        private readonly ConfigurationManager _confgurationManager;
        private readonly RepositoryClient[] _repositoryClients;

        public RepositoryManager(ConfigurationManager confgurationManager)
        {
            _confgurationManager = confgurationManager ?? throw new ArgumentNullException(nameof(confgurationManager));
            _repositoryClients = new RepositoryClient[]
            {
                new SvnRepositoryClient(),
                new GitRepositoryClient(), 
            };
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
        
        public void AddRepository(string repositoryName, VersionSystem repositoryVcs, Uri repositoryUri)
        {
            if (string.IsNullOrWhiteSpace(repositoryName))
                throw new ArgumentNullException(nameof(repositoryName));
            if (repositoryUri == null)
                throw new ArgumentNullException(nameof(repositoryUri));

            _confgurationManager.UpdateRepositoryConfig(repositories =>
            {
                if (repositories.Any(r => r.Name == repositoryName))
                    throw new ArgumentException("Repository already exists");

                var repository = new Repository(repositoryName, repositoryVcs, repositoryUri);
                repositories.Add(repository);
            });
        }

        public IEnumerable<Repository> GetRepositories(string name = null)
        {
            var repositories = _confgurationManager.GetRepositoryConfig()
                .Where(r => name == null || r.Name == name)
                .ToList();
            if (name != null && !repositories.Any())
                throw new ArgumentException("Repository was not found");
            return repositories;
        }
    }
}
