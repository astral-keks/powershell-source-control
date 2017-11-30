using AstralKeks.SourceControl;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.Common
{
    public class SourceControlCmdlet : Cmdlet
    {
        protected readonly ComponentContainer Components = new ComponentContainer();
    }

    public class SourceControlPSCmdlet : PSCmdlet
    {
        protected readonly ComponentContainer Components = new ComponentContainer();
    }
}
