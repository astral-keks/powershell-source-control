using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.Reset, Noun.SCWorkingCopy)]
    public class ResetWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessPath(string path)
        {
            Components.WorkingCopyController.ResetWorkingCopy(path);
        }
    }
}
