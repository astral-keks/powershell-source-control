using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.Show, Noun.SCWorkingCopyLog)]
    public class ShowWorkingCopyLogCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Components.WorkingCopyController.ShowWorkingCopyLog(Query);
        }
    }
}
