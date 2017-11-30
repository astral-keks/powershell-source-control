using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.Add, Noun.SCWorkingCopyItem)]
    public class AddWorkingItemCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessPath(string path)
        {
            Components.WorkingCopyController.AddWorkingCopyEntry(path);
        }
    }
}
