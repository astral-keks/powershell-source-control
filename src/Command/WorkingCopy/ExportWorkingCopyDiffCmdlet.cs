using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsData.Export, Noun.SCWorkingCopyDiff)]
    [OutputType(typeof(string))]
    public class ExportWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessPath(string path)
        {
            var diffPaths = Components.WorkingCopyController.ExportWorkingCopyDiff(path);
            WriteObject(diffPaths, true);
        }
    }
}
