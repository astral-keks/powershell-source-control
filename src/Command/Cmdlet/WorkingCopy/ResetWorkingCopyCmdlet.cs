using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Reset, Noun.SCWorkingCopy)]
    public class ResetWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.ResetWorkingCopy(Query);
        }
    }
}
