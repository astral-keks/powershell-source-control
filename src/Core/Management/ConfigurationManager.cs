using AstralKeks.SourceControl.Core.Data;
using AstralKeks.SourceControl.Core.Resources;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Resources;
using System;
using System.Collections.Generic;

namespace AstralKeks.SourceControl.Core.Management
{
    public class ConfigurationManager
    {
        private readonly ResourceManager _resourceManager;

        public ConfigurationManager(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }

        public List<Repository> GetRepositoryConfig()
        {
            var repositoriesResource = GetConfigResource(Files.Repository);
            var repositories = repositoriesResource.Read<List<Repository>>();
            return repositories;
        }

        public void UpdateRepositoryConfig(Action<List<Repository>> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            var repositoriesResource  = GetConfigResource(Files.Repository);
            var repositories = repositoriesResource.Read<List<Repository>>();
            try
            {
                callback(repositories);
            }
            finally
            {
                repositoriesResource.Write(repositories);
            }
        }

        public List<ShortcutPattern> GetAliasConfig()
        {
            var aliasPatternsResource = GetConfigResource(Files.Shortcut);
            var aliasPatterns = aliasPatternsResource.Read<List<ShortcutPattern>>();
            return aliasPatterns;
        }
        
        private Resource GetConfigResource(string fileName)
        {
            var locations = new[] { Paths.WorkspaceDirectory, Paths.UserspaceDirectory };
            return _resourceManager.CreateResource(locations, Directories.Config, fileName);
        }

    }
}
