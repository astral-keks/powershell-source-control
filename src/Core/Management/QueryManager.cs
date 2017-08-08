using AstralKeks.SourceControl.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AstralKeks.SourceControl.Core.Management
{
    public class QueryManager
    {
        private readonly WorkingCopyManager _workingCopyManager;

        public QueryManager(WorkingCopyManager workingCopyManager)
        {
            _workingCopyManager = workingCopyManager ?? throw new ArgumentNullException(nameof(workingCopyManager));
        }

        public string[] CompleteQuery(string query)
        {
            var parsedQuery = ParseQuery(query);

            var expandedQuery = !string.IsNullOrWhiteSpace(parsedQuery.Directory)
                ? ExpandQuery(parsedQuery.Directory, parsedQuery.FileName)
                : ExpandQuery(parsedQuery.FileName);

            return expandedQuery.Distinct().ToArray();
        }

        private IEnumerable<string> ExpandQuery(string queryDirectory, string queryFileName)
        {
            var queryDirectoryPath = new WorkingPathBuilder(queryDirectory).WorkingPath;
            var binding = new WorkingPathBinding(_workingCopyManager.GetWorkingCopies());
            foreach (var unboundDirectory in binding.Unbind(queryDirectoryPath).Select(p => p.ToString()))
            {
                foreach (var expandedQueryPath in Directory.EnumerateFileSystemEntries(unboundDirectory))
                {
                    var fileName = Path.GetFileName(expandedQueryPath);
                    if (fileName.StartsWith(queryFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        var expandedQuery = WorkingPathBuilder.Combine(queryDirectory, fileName);
                        if (Directory.Exists(expandedQueryPath))
                            expandedQuery = $"{expandedQuery}{Path.DirectorySeparatorChar}";

                        yield return expandedQuery;
                    };
                }
            }
        }

        private IEnumerable<string> ExpandQuery(string queryFileName)
        {
            foreach (var expandedQuery in _workingCopyManager.GetWorkingCopies().Select(w => w.RootPath))
            {
                if (expandedQuery.StartsWith(queryFileName, StringComparison.OrdinalIgnoreCase))
                    yield return $"{expandedQuery}{Path.DirectorySeparatorChar}";
            }
        }

        private (string Directory, string FileName) ParseQuery(string query)
        {
            var index = query?.LastIndexOf(Path.DirectorySeparatorChar) ?? -1;
            return (query.Substring(0, index + 1), query.Substring(index + 1));
        }
    }
}
