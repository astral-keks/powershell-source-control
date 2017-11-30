﻿using AstralKeks.SourceControl.Command.Common;
using System.Management.Automation;

namespace AstralKeks.SourceControl.Command.WorkingCopy
{
    [Cmdlet(VerbsCommon.Show, Noun.SCWorkingCopyDiff)]
    public class ShowWorkingCopyDiffCmdlet : WorkingCopyCmdlet
    {
        protected override void ProcessRecord()
        {
            Components.WorkingCopyController.ShowWorkingCopyDiff(Query);
        }
    }
}
