using AstralKeks.SourceControl.Core.Management;
using AstralKeks.Workbench.Common.Resources;

namespace AstralKeks.SourceControl.Core
{
    public class SourceControlEnvironment
    {
        private readonly RepositoryManager _repositoryManager;
        private readonly WorkingCopyManager _workingCopyManager;
        private readonly ShortcutIndex _shortcutIndex;
        private readonly QueryManager _queryManager;

        public SourceControlEnvironment()
        {
            var resourceManager = new ResourceManager(typeof(SourceControlEnvironment));
            var configurationManager = new ConfigurationManager(resourceManager);

            _repositoryManager = new RepositoryManager(configurationManager);
            _workingCopyManager = new WorkingCopyManager(_repositoryManager);
            _queryManager = new QueryManager(_workingCopyManager);
            _shortcutIndex = new ShortcutIndex(configurationManager);
        }

        public RepositoryManager RepositoryManager => _repositoryManager;
        public WorkingCopyManager WorkingCopyManager => _workingCopyManager;
        public QueryManager QueryManager => _queryManager;
        public ShortcutIndex ShortcutIndex => _shortcutIndex;
    }
}
