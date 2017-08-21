using System;
using System.Management.Automation;
using AstralKeks.SourceControl.Core.Data;
using AstralKeks.PowerShell.Common.Attributes;
using System.Linq;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Add, Noun.SCRepository)]
    public class AddRepositoryCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Position = 0, Mandatory = true)]
        [ValidatePattern(Validation.RepositoryNamePattern)]
        public string RepositoryName => Parameters.GetValue<string>(nameof(RepositoryName));

        [DynamicParameter(Position = 1, Mandatory = true)]
        public VersionSystem RepositoryVCS => Parameters.GetValue<VersionSystem>(nameof(RepositoryVCS));

        [DynamicParameter(Position = 2, Mandatory = true)]
        [ValidateNotNullOrEmpty, DynamicCompleter(nameof(GetRepositoryUris))]
        public string RepositoryUri => Parameters.GetValue<string>(nameof(RepositoryUri));

        protected override void ProcessRecord()
        {
            Env.RepositoryManager.AddRepository(RepositoryName, RepositoryVCS, new Uri(RepositoryUri));
        }

        public string[] GetRepositoryUris(string pattern)
        {
            return Env.RepositoryManager.GetRepositories()
                .Where(r => r.Uri.ToString().Contains(pattern))
                .Select(r => r.Uri.ToString())
                .ToArray();
        }
    }
}
