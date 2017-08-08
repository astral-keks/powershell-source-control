
using System;
using AstralKeks.SourceControl.Core.Data;

namespace AstralKeks.SourceControl.Core.Client
{
    public abstract class RepositoryClient
    {
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
