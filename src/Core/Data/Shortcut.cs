
using System;
using System.IO;

namespace AstralKeks.SourceControl.Core.Data
{
    public class Shortcut
    {
        public Shortcut()
        {
        }

        public Shortcut(string targetPath, string workingCopyName)
        {
            if (string.IsNullOrWhiteSpace(targetPath) || !Path.IsPathRooted(targetPath))
                throw new ArgumentException("Shortcut target path is invalid");
            if (string.IsNullOrWhiteSpace(workingCopyName))
                throw new ArgumentException("Shortcut working copy name is invalid");

            Name = $"{Path.GetFileName(targetPath)}@{workingCopyName}";
            TargetPath = targetPath;
        }

        public string Name { get; set; }

        public string TargetPath { get; set; }
    }
}
