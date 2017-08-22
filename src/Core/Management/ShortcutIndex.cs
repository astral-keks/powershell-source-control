using AstralKeks.SourceControl.Core.Data;
using AstralKeks.SourceControl.Core.Resources;
using AstralKeks.Workbench.Common.FileSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AstralKeks.SourceControl.Core.Management
{
    public class ShortcutIndex
    {
        private readonly ConfigurationManager _confgurationManager;

        public ShortcutIndex(ConfigurationManager confgurationManager)
        {
            _confgurationManager = confgurationManager ?? throw new ArgumentNullException(nameof(confgurationManager));
        }

        public void Sync(WorkingCopy workingCopy)
        {
            var aliasPatterns = _confgurationManager.GetAliasConfig();

            var projects = EnumerateEntries(workingCopy.Name, workingCopy.OriginPath, aliasPatterns).ToList();
            WriteIndex(workingCopy.Name, projects);
        }
        
        public IEnumerable<Shortcut> Search(string shortcutName = null)
        {
            return ReadIndex().Where(shortcut => shortcutName == null || shortcut.Name == shortcutName);
        }

        private IEnumerable<Shortcut> EnumerateEntries(string workingCopyName, string directory, List<ShortcutPattern> patterns)
        {
            foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
            {
                if (patterns.Any(t => IsPatternMatching(t, entry)))
                {
                    yield return new Shortcut(entry, workingCopyName);
                }
                else if (Directory.Exists(entry))
                {
                    foreach (var childEntry in EnumerateEntries(workingCopyName, entry, patterns))
                        yield return childEntry;
                }
            }
        }

        private bool IsPatternMatching(ShortcutPattern pattern, string path)
        {
            var regex = WildcardToRegex(pattern.PathPattern);
            if (!pattern.ChildPathPatterns.Any() && Regex.IsMatch(path, regex))
                return true;

            if (Directory.Exists(path) && pattern.ChildPathPatterns.Any())
            {
                var subdirRegexes = pattern.ChildPathPatterns.Select(WildcardToRegex).ToDictionary(r => r, r => false);
                foreach (var entry in Directory.EnumerateFileSystemEntries(path))
                {
                    var matchingRegex = subdirRegexes.Keys.FirstOrDefault(r => Regex.IsMatch(entry, r));
                    if (matchingRegex != null)
                        subdirRegexes[matchingRegex] = true;
                }

                return subdirRegexes.Values.All(v => v);
            }

            return false;
        }

        private string WildcardToRegex(string pattern)
        {
            return $"^{Regex.Escape(pattern).Replace(@"\*", ".*").Replace(@"\?", ".")}$";
        }

        private IEnumerable<Shortcut> ReadIndex()
        {
            var indexFiles = Directory.GetFiles(Paths.IndexDirectory)
                .Where(f => f.EndsWith(Files.Index))
                .ToList();

            foreach (var indexFilename in indexFiles)
            {
                var json = File.ReadAllText(indexFilename);
                var shorcuts = JsonConvert.DeserializeObject<List<Shortcut>>(json);
                foreach (var shortcut in shorcuts)
                    yield return shortcut;
            }
        }

        private void WriteIndex(string workingCopyName, List<Shortcut> entries)
        {
            var indexFilename = InitializeIndex(workingCopyName);
            var json = JsonConvert.SerializeObject(entries, Formatting.Indented);
            File.WriteAllText(indexFilename, json);
        }

        private string InitializeIndex(string workingCopyName)
        {
            var fileName = $"{workingCopyName}{Files.Index}";
            var indexPath = FsPath.Absolute(Paths.IndexDirectory, fileName);
            var indexDirectory = Path.GetDirectoryName(indexPath);
            if (!Directory.Exists(indexDirectory))
                Directory.CreateDirectory(indexDirectory);

            return indexPath;
        }
    }
}
