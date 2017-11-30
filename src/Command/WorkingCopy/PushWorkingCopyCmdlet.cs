using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.Push, Noun.SCWorkingCopy)]
    public class PushWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessPath(string path)
        {
            Components.WorkingCopyController.PushWorkingCopy(path);
        }
    }
}
