using System;
using System.IO;

namespace AstralKeks.SourceControl.Models
{
    public class WorkingCopy
    {
        public WorkingCopy(string name, string originPath)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (originPath == null)
                throw new ArgumentNullException(nameof(originPath));
            if (!Path.IsPathRooted(originPath))
                throw new ArgumentException($"{nameof(originPath)} is not absolute");

            Name = name;
            RootPath = $"{name}:";
            OriginPath = originPath
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)
                .Trim(Path.DirectorySeparatorChar);
        }

        public string Name { get; }

        public string RootPath { get; }

        public string OriginPath { get; }
    }
}
