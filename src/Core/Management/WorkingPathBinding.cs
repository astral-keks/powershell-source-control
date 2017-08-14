using AstralKeks.SourceControl.Core.Data;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.SourceControl.Core.Management
{
    public class WorkingPathBinding
    {
        private delegate void Binder();
        private delegate void Unbinder();
        private readonly List<WorkingCopy> _workingCopies;

        public WorkingPathBinding(IEnumerable<WorkingCopy> workingCopies)
        {
            if (workingCopies == null)
                throw new ArgumentNullException(nameof(workingCopies));

            _workingCopies = workingCopies.ToList();
        }

        public IEnumerable<WorkingPath> Bind(WorkingPath path)
        {
            path = NormalizePath(path);

            foreach (var workingCopy in GetWorkingCopies(path))
            {
                var pathBuilder = new WorkingPathBuilder(path);
                pathBuilder.FullPath = ReplacePath(pathBuilder.FullPath, workingCopy.OriginPath, workingCopy.RootPath);
                pathBuilder.RootPath = workingCopy.RootPath;
                yield return pathBuilder.WorkingPath;
            }
        }

        public IEnumerable<WorkingPath> Unbind(WorkingPath path)
        {
            path = NormalizePath(path);

            foreach (var workingCopy in GetWorkingCopies(path))
            {
                var pathBuilder = new WorkingPathBuilder(path);
                pathBuilder.FullPath = ReplacePath(pathBuilder.FullPath, workingCopy.RootPath, workingCopy.OriginPath);
                yield return pathBuilder.WorkingPath;
            }
        }

        private IEnumerable<WorkingCopy> GetWorkingCopies(WorkingPath workingPath)
        {
            var success = false;
            var path = workingPath.ToString();
            foreach (var workingCopy in _workingCopies)
            {
                if (path.Equals(workingCopy.OriginPath, StringComparison.OrdinalIgnoreCase)
                    || path.ToString().StartsWith(workingCopy.OriginPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) 
                    || Operators.LikeString(workingCopy.RootPath, workingPath.RootPath, CompareMethod.Text))
                {
                    success = true;
                    yield return workingCopy;
                }
            }

            if (!success)
                throw new DirectoryNotFoundException($"Working copy was not found for {workingPath}");
        }
        
        private WorkingPath NormalizePath(WorkingPath path)
        {
            var pathBuilder = new WorkingPathBuilder(path);

            if (string.IsNullOrWhiteSpace(pathBuilder.RootPath))
                pathBuilder.RootPath = WorkingPath.WildcardBasePath;
            if (pathBuilder.RootPath == WorkingPath.CurrentBasePath)
                pathBuilder.FullPath = Path.GetFullPath(pathBuilder.FullPath);

            return pathBuilder.WorkingPath;
        }

        private string ReplacePath(string path, string oldPart, string newPart)
        {
            var index = -1;
            if (!string.IsNullOrEmpty(oldPart))
                index = path.IndexOf(oldPart, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
                path = $"{path.Substring(0, index)}{newPart}{path.Substring(index + oldPart.Length)}";
            return path;
        }
    }
}
