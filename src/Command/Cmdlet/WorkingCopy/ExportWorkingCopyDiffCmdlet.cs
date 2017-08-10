using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsData.Export, Noun.SCWorkingCopyDiff)]
    [OutputType(typeof(string))]
    public class ExportWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            var diffPaths = Env.WorkingCopyManager.ExportWorkingCopyDiff(Query);
            WriteObject(diffPaths, true);
        }
    }
}
