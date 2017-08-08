using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Add, Noun.WorkingItem)]
    public class AddWorkingItemCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.AddWorkingCopyEntry(Query);
        }
    }
}
