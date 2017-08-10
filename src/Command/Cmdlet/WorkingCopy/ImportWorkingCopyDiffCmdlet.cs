using AstralKeks.Workbench.PowerShell.Attributes;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsData.Import, Noun.SCWorkingCopyDiff)]
    public class ImportWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        [DynamicParameter(Position = 1, ValueFromPipeline = true)]
        public string WorkingCopyDiffPath => Parameters.GetValue<string>(nameof(WorkingCopyDiffPath));

        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.ImportWorkingCopyDiff(Query, WorkingCopyDiffPath);
        }
    }
}
