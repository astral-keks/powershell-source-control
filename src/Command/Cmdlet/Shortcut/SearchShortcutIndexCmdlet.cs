using AstralKeks.Workbench.PowerShell.Attributes;
using System;
using System.Linq;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command
{
    [Cmdlet(VerbsCommon.Search, Noun.SCShortcutIndex)]
    [OutputType(typeof(string))]
    public class SearchShortcutIndexCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Mandatory = true, Position = 0)]
        [DynamicCompleter(nameof(GetShortcutNames))]
        public string ShortcutName => Parameters.GetValue<string>(nameof(ShortcutName));

        protected override void ProcessRecord()
        {
            WriteObject(Env.ShortcutIndex.Search(ShortcutName).Select(p => p.TargetPath), true);
        }

        public string[] GetShortcutNames(string shortcutNamePart)
        {
            return Env.ShortcutIndex.Search()
                .Select(s => new
                {
                    Name = s.Name,
                    Index = s.Name.IndexOf(shortcutNamePart, StringComparison.OrdinalIgnoreCase)
                })
                .Where(t => t.Index >= 0)
                .OrderBy(t => t.Index)
                .Select(r => r.Name)
                .ToArray();
        }
    }
}
