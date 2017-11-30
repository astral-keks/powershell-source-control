using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsData.Mount, Noun.SCWorkingCopyDrive)]
    public class MountWorkingCopyDriveCmdlet : SourceControlPSCmdlet
    {
        protected override void ProcessRecord()
        {
            var workingCopies = Components.WorkingCopyManager.GetWorkingCopies();
            foreach (var workingCopy in workingCopies)
            {
                var script = $"New-PSDrive -name {workingCopy.Name} -psprovider FileSystem -root {workingCopy.OriginPath} -Scope Global";
                SessionState.InvokeCommand.InvokeScript(script);
            }
        }
    }
}
