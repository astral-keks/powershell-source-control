using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsData.Import, Noun.SCWorkingCopyDiff)]
    public class ImportWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        [Parameter(Position = 1, ValueFromPipeline = true)]
        public string WorkingCopyDiffPath { get; set; }

        protected override void ProcessPath(string path)
        {
            Components.WorkingCopyController.ImportWorkingCopyDiff(path, WorkingCopyDiffPath);
        }
    }
}
