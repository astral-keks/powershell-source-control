using System.Linq;
using System.Management.Automation;
using AstralKeks.Workbench.PowerShell.Attributes;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsData.Sync, Noun.ShortcutIndex)]
    public class SyncShortcutIndexCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Mandatory = true, Position = 0)]
        [ValidateDynamicSet(nameof(GetParameterValues))]
        public string WorkingCopyName => Parameters.GetValue<string>(nameof(WorkingCopyName));
        
        protected override void ProcessRecord()
        {
            var workingCopy = Env.WorkingCopyManager.GetWorkingCopy(WorkingCopyName);
            Env.ShortcutManager.Sync(workingCopy);
        }

        public string[] GetParameterValues()
        {
            return Env.WorkingCopyManager.GetWorkingCopies().Select(r => r.Name).ToArray();
        }
    }
}
