using AstralKeks.SourceControl.Models;
using AstralKeks.SourceControl.Resources;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using AstralKeks.Workbench.Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.SourceControl.Managers
{
    public class WorkingCopyManager
    {
        private readonly FileSystem _fileSystem;
        private readonly SharedContext _sharedContext;
        private readonly RepositoryManager _repositoryManager;

        public WorkingCopyManager(FileSystem fileSystem, SharedContext sharedContext, RepositoryManager repositoryManager)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
        }

        public WorkingCopy CreateWorkingCopy(string workingCopyName, string repositoryName)
        {
            if (string.IsNullOrWhiteSpace(workingCopyName))
                throw new ArgumentNullException(nameof(workingCopyName));
            if (string.IsNullOrWhiteSpace(repositoryName))
                throw new ArgumentNullException(nameof(repositoryName));

            var repository = _repositoryManager.GetRepositories(repositoryName).First();

            var workingCopy = DefineWorkingCopy(workingCopyName, null);
            var client = _repositoryManager.GetRepositoryClient(repository.Vcs);
            client.CreateWorkingCopy(workingCopy.OriginPath, repository.Uri);
            return workingCopy;
        }
        
        public WorkingCopy GetWorkingCopy(string workingCopyName)
        {
            if (string.IsNullOrWhiteSpace(workingCopyName))
                throw new ArgumentException("Working copy name is invalid");

            var workingCopy = DefineWorkingCopy(workingCopyName, null);
            if (workingCopy == null)
                throw new DirectoryNotFoundException($"Working copy {workingCopyName} was not found");

            return workingCopy;
        }
        
        public IEnumerable<WorkingCopy> GetWorkingCopies()
        {
            var sourceDirectory = ResolveSourceDirectory();
            var entries = _fileSystem.DirectoryList(sourceDirectory);
            return entries
                .Where(e => _fileSystem.DirectoryExists(e))
                .Where(d => _repositoryManager.GetRepositoryClient(d) != null)
                .Select(d => DefineWorkingCopy(null, d));
        }

        private WorkingCopy DefineWorkingCopy(string workingCopyName, string workingCopyPath)
        {
            WorkingCopy workingCopy = null;

            if (!string.IsNullOrWhiteSpace(workingCopyName) || !string.IsNullOrWhiteSpace(workingCopyPath))
            {
                if (string.IsNullOrWhiteSpace(workingCopyName))
                    workingCopyName = Path.GetFileName(workingCopyPath);
                if (string.IsNullOrWhiteSpace(workingCopyPath))
                    workingCopyPath = PathBuilder.Combine(ResolveSourceDirectory(), workingCopyName);

                workingCopy = new WorkingCopy(workingCopyName, workingCopyPath);
            }

            return workingCopy;
        }

        private string ResolveSourceDirectory()
        {
            var sourceDirectoryRoot = _sharedContext.CurrentWorkspaceDirectory;
            if (string.IsNullOrWhiteSpace(sourceDirectoryRoot))
                sourceDirectoryRoot = _sharedContext.CurrentUserspaceDirectory;
            return PathBuilder.Combine(sourceDirectoryRoot, Directories.Source);
        }
    }
}
