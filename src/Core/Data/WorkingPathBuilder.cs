using AstralKeks.SourceControl.Core.Resources;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AstralKeks.SourceControl.Core.Data
{
    public class WorkingPathBuilder
    {
        private static readonly Regex _rootPathValidator = new Regex(Patterns.WorkingPathRoot, RegexOptions.Compiled);
        private static readonly Regex _relativePathValidator = new Regex(Patterns.WorkingPathRelative, RegexOptions.Compiled);
        private static readonly Regex _fullPathValidator = new Regex(Patterns.WorkingPathFull, RegexOptions.Compiled);

        private string _rootPath;
        private string _relativePath;

        public WorkingPathBuilder(WorkingPath path)
        {
            FullPath = path?.ToString();
        }

        public WorkingPathBuilder(string path)
        {
            FullPath = path;
        }

        public string RootPath
        {
            get { return _rootPath; }
            set
            {
                value = TransformSeparator(value);
                var match = _rootPathValidator.Match(value);
                if (!match.Success)
                    throw new FormatException($"Root path {value} has invalid format");

                _rootPath = match.Groups[Patterns.WorkingPathRootGroup].Value;
            }
        }

        public string RelativePath
        {
            get { return _relativePath; }
            set
            {
                value = TransformSeparator(value);
                var match = _relativePathValidator.Match(value);
                if (!match.Success)
                    throw new FormatException($"Relative path {value} has invalid format");

                _relativePath = match.Groups[Patterns.WorkingPathRelativeGroup].Value;
            }
        }

        public string FullPath
        {
            get { return Combine(_rootPath, _relativePath); }
            set
            {
                value = TransformSeparator(value);
                var match = _fullPathValidator.Match(value);
                if (!_fullPathValidator.IsMatch(value))
                    throw new FormatException($"Full path {value} has invalid format");

                _rootPath = match.Groups[Patterns.WorkingPathRootGroup].Value;
                _relativePath = match.Groups[Patterns.WorkingPathRelativeGroup].Value;
            }
        }

        public WorkingPath WorkingPath => new WorkingPath(_rootPath, _relativePath);

        public static string TransformSeparator(string uri)
        {
            uri = uri?.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Trim(Path.DirectorySeparatorChar);
            uri = uri ?? string.Empty;
            return uri;
        }

        public static string Combine(string firstPath, string secondPath)
        {
            firstPath = firstPath.EndsWith(":") && !string.IsNullOrWhiteSpace(secondPath) 
                ? $"{firstPath}{Path.DirectorySeparatorChar}" 
                : firstPath;
            return Path.Combine(firstPath, secondPath);
        }

        public static string Validate(string path)
        {
            if (File.Exists(path))
            {
                var pathParts = path.Split(Path.DirectorySeparatorChar);
                if (pathParts.Any())
                {
                    var validPath = pathParts.First();
                    for (int i = 1; i < pathParts.Length; i++)
                    {
                        if (!validPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                            validPath += Path.DirectorySeparatorChar;

                        var entries = Directory.GetFileSystemEntries(validPath, pathParts[i]);
                        if (entries.Length == 1)
                        {
                            var validPathPart = entries.Single().Replace(validPath, string.Empty);
                            validPath = Combine(validPath, validPathPart);
                        }
                        else
                            break;
                    }

                    if (validPath.Length == path.Length)
                        path = validPath;
                }
            }

            return path;
        }
    }
}
