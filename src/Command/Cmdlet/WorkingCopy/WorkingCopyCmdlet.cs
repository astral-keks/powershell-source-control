using AstralKeks.PowerShell.Common.Attributes;

namespace AstralKeks.SourceControl.Command
{
    public class WorkingCopyCmdlet : SourceControlDynamicCmdlet
    {
        [DynamicParameter(Position = 0, ValueFromPipeline = true)]
        [DynamicCompleter(nameof(CompleteWorkingItemQuery))]
        public string Query => Parameters.GetValue<string>(nameof(Query));
        
        public string[] CompleteWorkingItemQuery(string queryPart)
        {
            var options = Env.QueryManager.CompleteQuery(queryPart);
            return options;
        }
    }
}
