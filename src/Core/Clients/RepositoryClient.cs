using System;
using AstralKeks.SourceControl.Models;
using AstralKeks.Workbench.Common.Infrastructure;

namespace AstralKeks.SourceControl.Clients
{
    public abstract class RepositoryClient
    {
        public RepositoryClient(ProcessLauncher processLauncher)
        {
            ProcessLauncher = processLauncher ?? throw new ArgumentNullException(nameof(processLauncher));
        }

        protected ProcessLauncher ProcessLauncher { get; }

        public abstract VersionSystem Vcs { get; }

        public abstract bool IsWorkingCopy(string directory);

        public abstract void CreateWorkingCopy(string directory, Uri uri);

        public abstract void PopWorkingCopy(string path);

        public abstract void PushWorkingCopy(string path);

        public abstract void ResetWorkingCopy(string path);

        public abstract void ShowWorkingCopyDiff(string path);

        public abstract void ShowWorkingCopyLog(string path);

        public abstract void AddWorkingCopyItem(string path);

        public abstract void ExportWorkingCopyDiff(string path, string diffPath);

        public abstract void ImportWorkingCopyDiff(string path, string diffPath);
    }
}
