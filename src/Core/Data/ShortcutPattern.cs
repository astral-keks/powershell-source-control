using System.Collections.Generic;

namespace AstralKeks.SourceControl.Core.Data
{
    public class ShortcutPattern
    {
        public string PathPattern { get; set; }

        public List<string> ChildPathPatterns { get; set; } = new List<string>();
    }
}
