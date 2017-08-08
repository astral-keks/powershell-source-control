using System.IO;

namespace AstralKeks.SourceControl.Core.Data
{
    public class WorkingPath
    {
        public const string WildcardBasePath = "*:";
        public const string CurrentBasePath = ".";

        private readonly string _root;
        private readonly string _rootPath;
        private readonly string _relativePath;
        private readonly string _fullPath;

        public WorkingPath(string rootPath, string relativePath = null)
        {
            _rootPath = WorkingPathBuilder.TransformSeparator(rootPath);
            _relativePath = WorkingPathBuilder.TransformSeparator(relativePath);
            _fullPath = WorkingPathBuilder.Combine(_rootPath, _relativePath);
            _root = _rootPath.Trim(':');
        }

        public string Root => _root;

        public string RootPath => _rootPath;

        public string RelativePath => _relativePath;
        
        public override string ToString()
        {
            return _fullPath;
        }
    }
}
