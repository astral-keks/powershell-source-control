using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Push, Noun.SCWorkingCopy)]
    public class PushWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.PushWorkingCopy(Query);
        }
    }
}
