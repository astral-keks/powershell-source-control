using AstralKeks.PowerShell.Common.Attributes;
using AstralKeks.PowerShell.Common.Parameters;
using AstralKeks.SourceControl.Command.Common;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.New, Noun.SCWorkingCopy)]
    public class NewWorkingCopyCmdlet : SourceControlCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string WorkingCopyName { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        [ArgumentCompleter(typeof(ParameterCompleter))]
        public string RepositoryName { get; set; }

        protected override void ProcessRecord()
        {
            Components.WorkingCopyManager.CreateWorkingCopy(WorkingCopyName, RepositoryName);
        }

        [ParameterCompleter(nameof(RepositoryName))]
        public IEnumerable<string> CompleteRepositoryName(string repositoryNamePart)
        {
            return Components.RepositoryManager.GetRepositories().Select(r => r.Name).ToArray();
        }
    }
}
