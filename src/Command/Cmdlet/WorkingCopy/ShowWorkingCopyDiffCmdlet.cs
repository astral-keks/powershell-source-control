using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Show, Noun.SCWorkingCopyDiff)]
    public class ShowWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.ShowWorkingCopyDiff(Query);
        }
    }
}
