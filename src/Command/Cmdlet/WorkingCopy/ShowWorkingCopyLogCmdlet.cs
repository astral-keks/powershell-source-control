using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Show, Noun.SCWorkingCopyLog)]
    public class ShowWorkingCopyLogCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Env.WorkingCopyManager.ShowWorkingCopyLog(Query);
        }
    }
}
