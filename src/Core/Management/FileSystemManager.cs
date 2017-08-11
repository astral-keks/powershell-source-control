using AstralKeks.SourceControl.Core.Resources;
using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.FileSystem;
using System;
using System.IO;

namespace AstralKeks.SourceControl.Core.Management
{
    public class FileSystemManager
    {    
        public string SourceDirectory => FsPath.Absolute(Location.Workspace(), Directories.Source);

        public string WorkingCopyIndexDirectory => FsPath.Absolute(Location.Workspace(), Directories.Temp, Directories.Index);

        public string WorkingCopyDiffPath()
        {
            var relativePath = Path.Combine(Directories.Temp, Directories.Diff);
            var fileName = $"{DateTime.UtcNow.ToString("yy-MM-dd-HH-mm-ss-ffff")}.patch";
            return FsPath.Absolute(Location.Workspace(), relativePath, fileName);
        }

        public string WorkingCopyIndexPath(string workingCopyName)
        {
            var relativePath = Path.Combine(Directories.Temp, Directories.Index);
            var fileName = $"{workingCopyName}{Files.Index}";
            return FsPath.Absolute(Location.Workspace(), relativePath, fileName);
        }

        public string WorkingCopyPath(string workingCopyName)
        {
            return FsPath.Absolute(Location.Workspace(), Directories.Source, workingCopyName);
        }

        internal bool Exists(string path)
        {
            return Directory.Exists(path) || File.Exists(path);
        }
    }
}
