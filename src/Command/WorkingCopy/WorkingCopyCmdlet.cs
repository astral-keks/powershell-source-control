using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.PowerShell.Common.Extensions;
using AstralKeks.SourceControl.Command.Common;
using System;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    public class WorkingCopyCmdlet : SourceControlPSCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public string Query { get; set; }

        protected string[] ResolvedQuery => string.IsNullOrWhiteSpace(Query)
            ? Components.WorkingCopyManager.GetWorkingCopies().Select(wc => wc.RootPath).ToArray()
            : new[] { Query };

        protected override void ProcessRecord()
        {
            var resolvedPaths = ResolvedQuery
                .SelectMany(SessionState.Path.GetResolvedProviderPathFromPSPath)
                .ToArray();
            foreach (var resolvedPath in resolvedPaths)
            {
                ProcessPath(resolvedPath);
            }
        }

        protected virtual void ProcessPath(string path)
        {

        }

        [ParameterCompleter(nameof(Query))]
        public string[] CompleteWorkingItemQuery(string queryPart)
        {
            if (queryPart?.Contains(":") == true || queryPart?.StartsWith(".") == true)
                throw new Exception("Feel free to do what you want, PowerShell;)");
            
            return Components.WorkingCopyManager.GetWorkingCopies().Select(wc => wc.RootPath).ToArray();
        }
    }
}
