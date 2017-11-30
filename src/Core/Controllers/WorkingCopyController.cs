using System;
using System.Collections.Generic;
using AstralKeks.SourceControl.Clients;
using AstralKeks.SourceControl.Resources;
using AstralKeks.SourceControl.Managers;
using AstralKeks.Workbench.Common.Utilities;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.Infrastructure;
using System.Linq;
using AstralKeks.SourceControl.Models;

namespace AstralKeks.SourceControl.Controllers
{
    public class WorkingCopyController
    {
        private readonly FileSystem _fileSystem;
        private readonly SharedContext _sharedContext;
        private readonly WorkingCopyManager _workingCopyManager;
        private readonly RepositoryManager _repositoryManager;

        public WorkingCopyController(FileSystem fileSystem, SharedContext sharedContext, 
            WorkingCopyManager workingCopyManager, RepositoryManager repositoryManager)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _sharedContext = sharedContext ?? throw new ArgumentNullException(nameof(sharedContext));
            _workingCopyManager = workingCopyManager ?? throw new ArgumentNullException(nameof(workingCopyManager));
            _repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
        }

        public void PopWorkingCopy(string workingCopyPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.PopWorkingCopy(path));
        }

        public void PushWorkingCopy(string workingCopyPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.PushWorkingCopy(path));
        }

        public void ResetWorkingCopy(string workingCopyPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.ResetWorkingCopy(path));
        }

        public void ShowWorkingCopyLog(string workingCopyPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.ShowWorkingCopyLog(path));
        }

        public void ShowWorkingCopyDiff(string workingCopyPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.ShowWorkingCopyDiff(path));
        }

        public IEnumerable<string> ExportWorkingCopyDiff(string workingCopyPath)
        {
            return InvokeClient(workingCopyPath, (client, path) =>
            {
                var workingCopyDiffPath = PathBuilder.Combine(ResolveDiffDirectory(), Files.DiffPatch());
                client.ExportWorkingCopyDiff(path, workingCopyDiffPath);
                return workingCopyDiffPath;
            });
        }

        public void ImportWorkingCopyDiff(string workingCopyPath, string workingCopyDiffPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.ImportWorkingCopyDiff(path, workingCopyDiffPath));
        }

        public void AddWorkingCopyEntry(string workingCopyPath)
        {
            InvokeClient(workingCopyPath, (client, path) => client.AddWorkingCopyItem(path));
        }
        

        private void InvokeClient(string workingCopyPath, Action<RepositoryClient, string> action)
        {
            var workingCopies = ResolveWorkingCopies(workingCopyPath);
            foreach (var workingCopy in workingCopies)
            {
                var client = ResolveRepositoryClient(workingCopy);
                action(client, workingCopyPath);
            }
        }

        private IEnumerable<TResult> InvokeClient<TResult>(string workingCopyPath, Func<RepositoryClient, string, TResult> func)
        {
            var workingCopies = ResolveWorkingCopies(workingCopyPath);
            foreach (var workingCopy in workingCopies)
            {
                var client = ResolveRepositoryClient(workingCopy);
                yield return func(client, workingCopyPath);
            }
        }

        private List<WorkingCopy> ResolveWorkingCopies(string workingCopyPath)
        {
            workingCopyPath = _fileSystem.MakeAbsolute(workingCopyPath ?? ".");

            var workingCopies = _workingCopyManager.GetWorkingCopies(wc => workingCopyPath.StartsWith(wc.OriginPath));
            if (!workingCopies.Any())
                workingCopies = _workingCopyManager.GetWorkingCopies(wc => wc.OriginPath.StartsWith(workingCopyPath));

            return workingCopies;
        }

        private RepositoryClient ResolveRepositoryClient(WorkingCopy workingCopy)
        {
            var client = _repositoryManager.GetRepositoryClient(workingCopy.OriginPath);
            if (client == null)
                throw new ArgumentException($"Repository client was not found for workingCopyPath '{workingCopy.OriginPath}'");

            return client;
        }

        private string ResolveDiffDirectory()
        {
            var sourceDirectoryRoot = _sharedContext.CurrentWorkspaceDirectory;
            if (string.IsNullOrWhiteSpace(sourceDirectoryRoot))
                sourceDirectoryRoot = _sharedContext.CurrentUserspaceDirectory;
            return PathBuilder.Combine(sourceDirectoryRoot, Directories.SourceControl, Directories.Diff);
        }
    }
}
