using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsData.Export, Noun.SCWorkingCopyDiff)]
    [OutputType(typeof(string))]
    public class ExportWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            var diffPaths = Components.WorkingCopyController.ExportWorkingCopyDiff(Query);
            WriteObject(diffPaths, true);
        }
    }
}
