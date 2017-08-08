using AstralKeks.Workbench.PowerShell.Attributes;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Search, Noun.ShortcutIndex)]
    [OutputType(typeof(string))]
    public class SearchShortcutIndexCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Mandatory = true, Position = 0)]
        [ValidateDynamicSet(nameof(GetParameterValues))]
        public string ShortcutName => Parameters.GetValue<string>(nameof(ShortcutName));

        protected override void ProcessRecord()
        {
            WriteObject(Env.ShortcutManager.Search(ShortcutName).Select(p => p.TargetPath), true);
        }

        public string[] GetParameterValues()
        {
            return Env.ShortcutManager.Search().Select(r => r.Name).ToArray();
        }
    }
}
