using AstralKeks.SourceControl.Core.Resources;
using AstralKeks.Workbench.Common.Data;
using AstralKeks.Workbench.Common.FileSystem;
using System;
using System.IO;

namespace AstralKeks.SourceControl.Core.Management
{
    public class FileSystemManager
    {    
        public string WorkspaceDirectory => SystemVariable.WorkspaceDirectory ?? Directory.GetCurrentDirectory();

        public string UserspaceDirectory => SystemVariable.UserspaceDirectory ?? FsPath.Absolute(HomeDirectory(), Directories.SourceControl);

        public string SourceDirectory => FsPath.Absolute(WorkspaceDirectory, Directories.Source);

        public string WorkingCopyIndexDirectory => FsPath.Absolute(WorkspaceDirectory, Directories.Temp, Directories.Index);

        public string HomeDirectory()
        {
            switch(Platform.Current)
            {
                case Platform.Windows:
                    return SystemVariable.LocalAppData;
                case Platform.Unix:
                    return SystemVariable.Home;
                default:
                    throw new NotSupportedException("Unknown platform");
            }
        }

        public string WorkingCopyDiffPath()
        {
            var relativePath = Path.Combine(Directories.Temp, Directories.Diff);
            var fileName = $"{DateTime.UtcNow.ToString("yy-MM-dd-HH-mm-ss-ffff")}.patch";
            return FsPath.Absolute(WorkspaceDirectory, relativePath, fileName);
        }

        public string WorkingCopyIndexPath(string workingCopyName)
        {
            var relativePath = Path.Combine(Directories.Temp, Directories.Index);
            var fileName = $"{workingCopyName}{Files.Index}";
            return FsPath.Absolute(WorkspaceDirectory, relativePath, fileName);
        }

        public string WorkingCopyPath(string workingCopyName)
        {
            return FsPath.Absolute(WorkspaceDirectory, Directories.Source, workingCopyName);
        }

        internal bool Exists(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }
    }
}
