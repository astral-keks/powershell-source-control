using AstralKeks.SourceControl.Core;
using AstralKeks.Workbench.PowerShell;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    public class SourceControlCmdlet : Cmdlet
    {
        protected readonly SourceControlEnvironment Env = new SourceControlEnvironment();
    }

    public class SourceControlPSCmdlet : PSCmdlet
    {
        protected readonly SourceControlEnvironment Env = new SourceControlEnvironment();
    }

    public class SourceControlDynamicCmdlet : DynamicCmdlet
    {
        protected readonly SourceControlEnvironment Env = new SourceControlEnvironment();
    }

    public class SourceControlDynamicPSCmdlet : DynamicPSCmdlet
    {
        protected readonly SourceControlEnvironment Env = new SourceControlEnvironment();
    }
}
