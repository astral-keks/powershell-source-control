using AstralKeks.Workbench.Common.Context;
using AstralKeks.Workbench.Common.FileSystem;

namespace AstralKeks.SourceControl.Core.Resources
{
    internal class Paths
    {
        public static string WorkspaceDirectory => Location.Workspace();

        public static string UserspaceDirectory => Location.Userspace(Directories.SourceControl);

        public static string SourceDirectory => FsPath.Absolute(Location.Workspace(), Directories.Source);

        public static string IndexDirectory => FsPath.Absolute(Location.Workspace(), Directories.Temp, Directories.Index);

        public static string DiffDirectory => FsPath.Absolute(Location.Workspace(), Directories.Temp, Directories.Diff);
    }
}
