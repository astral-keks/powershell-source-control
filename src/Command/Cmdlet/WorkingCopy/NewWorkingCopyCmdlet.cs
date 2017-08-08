using System.Linq;
using System.Management.Automation;
using AstralKeks.Workbench.PowerShell.Attributes;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WorkingCopy)]
    public class NewWorkingCopyCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Mandatory = true, Position = 0)]
        [ValidateNotNullOrEmpty]
        public string WorkingCopyName => Parameters.GetValue<string>(nameof(WorkingCopyName));

        [DynamicParameter(Mandatory = true, Position = 1)]
        [ValidateDynamicSet(nameof(GetParameterValues))]
        public string RepositoryName => Parameters.GetValue<string>(nameof(RepositoryName));

        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.CreateWorkingCopy(WorkingCopyName, RepositoryName);
        }

        public string[] GetParameterValues(string command, string parameter)
        {
            return Env.RepositoryManager.GetRepositories().Select(r => r.Name).ToArray();
        }
    }
}
