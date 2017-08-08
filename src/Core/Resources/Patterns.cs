
namespace AstralKeks.SourceControl.Core.Resources
{
    internal class Patterns
    {
        public static readonly string WorkingPathAliasGroup = "alias";
        public static readonly string WorkingPathRootGroup = "basePath";
        public static readonly string WorkingPathRelativeGroup = "relativePath";

        private static readonly string _workingPathAlias = $"((?<{WorkingPathAliasGroup}>[^\\\\/]+)@)?";
        private static readonly string _workingPathRoot = $"(?<{WorkingPathRootGroup}>[^:\\\\/]+:|[.])?[\\\\/]?";
        private static readonly string _workingPathRelative = $"[\\\\/]?(?<{WorkingPathRelativeGroup}>([\\\\/]?[^\\\\/]+)*)[\\\\/]?";

        public static readonly string WorkingPathAlias = $"^{_workingPathRoot}$";
        public static readonly string WorkingPathRoot = $"^{_workingPathRoot}$";
        public static readonly string WorkingPathRelative = $"^{_workingPathRelative}$";
        public static readonly string WorkingPathFull = $"^{_workingPathRoot}{_workingPathRelative}$";
    }
}
