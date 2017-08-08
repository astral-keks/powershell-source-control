﻿using AstralKeks.SourceControl.Core.Management;
using AstralKeks.Workbench.Common.Resources;

namespace AstralKeks.SourceControl.Core
{
    public class SourceControlEnvironment
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly WorkingCopyManager _workingCopyManager;
        private readonly ShortcutIndex _shortcutManager;
        private readonly QueryManager _queryManager;

        public SourceControlEnvironment()
        {
            var fileSystemManager = new FileSystemManager();
            var resourceManager = new ResourceManager(typeof(SourceControlEnvironment));
            var configurationManager = new ConfigurationManager(fileSystemManager, resourceManager);

            _repositoryManager = new RepositoryManager(configurationManager);
            _workingCopyManager = new WorkingCopyManager(fileSystemManager, _repositoryManager);
            _queryManager = new QueryManager(_workingCopyManager);
            _shortcutManager = new ShortcutIndex(fileSystemManager, configurationManager);
        }

        public RepositoryManager RepositoryManager => _repositoryManager;
        public WorkingCopyManager WorkingCopyManager => _workingCopyManager;
        public QueryManager QueryManager => _queryManager;
        public ShortcutIndex ShortcutManager => _shortcutManager;
    }
}
