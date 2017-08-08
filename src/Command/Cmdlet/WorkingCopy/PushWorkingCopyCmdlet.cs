using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Push, Noun.WorkingCopy)]
    public class PushWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.PushWorkingCopy(Query);
        }
    }
}
