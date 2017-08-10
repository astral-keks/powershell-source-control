using System.Linq;
using System.Management.Automation;
using AstralKeks.SourceControl.Core.Data;
using AstralKeks.Workbench.PowerShell.Attributes;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Get, Noun.SCRepository)]
    [OutputType(typeof(Repository))]
    public class GetRepositoryCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Mandatory = true, Position = 0)]
        [ValidateDynamicSet(nameof(GetParameterValues))]
        public string RepositoryName => Parameters.GetValue<string>(nameof(RepositoryName));

        protected override void ProcessRecord()
        {
            WriteObject(Env.RepositoryManager.GetRepositories(RepositoryName), true);
        }

        public string[] GetParameterValues()
        {
            return Env.RepositoryManager.GetRepositories().Select(r => r.Name).ToArray();
        }
    }
}
