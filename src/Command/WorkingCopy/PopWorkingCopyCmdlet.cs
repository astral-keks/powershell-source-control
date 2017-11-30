using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.Pop, Noun.SCWorkingCopy)]
    public class PopWorkingCopyCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessPath(string path)
        {
            Components.WorkingCopyController.PopWorkingCopy(path);
        }
    }
}
