using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Pop, Noun.SCWorkingCopy)]
    public class PopWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.PopWorkingCopy(Query);
        }
    }
}
