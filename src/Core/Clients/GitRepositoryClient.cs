﻿using AstralKeks.SourceControl.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AstralKeks.Workbench.Common.Infrastructure;

namespace AstralKeks.SourceControl.Clients
{
    internal class GitRepositoryClient : RepositoryClient
    {
        public GitRepositoryClient(ProcessLauncher launcher) 
            : base(launcher)
        {
        }

        public override VersionSystem Vcs => VersionSystem.Git;

        public override bool IsWorkingCopy(string directory)
        {
            return Directory.Exists(Path.Combine(directory, ".git"));
        }

        public override void CreateWorkingCopy(string directory, Uri uri)
        {
            TortoiseGitProc("clone", new Dictionary<string, string>
            {
                { "path", directory },
                { "exactpath", null },
                { "url", uri.ToString() }
            });
        }

        public override void PopWorkingCopy(string path)
        {
            TortoiseGitProc("pull", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void PushWorkingCopy(string path)
        {
            TortoiseGitProc("commit", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ResetWorkingCopy(string path)
        {
            TortoiseGitProc("revert", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ShowWorkingCopyDiff(string path)
        {
            TortoiseGitProc("diff", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ShowWorkingCopyLog(string path)
        {
            TortoiseGitProc("log", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void AddWorkingCopyItem(string path)
        {
            TortoiseGitProc("add", new Dictionary<string, string>
            {
                { "path", path }
            }, 
            true);
        }

        public override void ExportWorkingCopyDiff(string path, string diffPath)
        {
            TortoiseGitProc("diff", new Dictionary<string, string>
            {
                { "path", path },
                { "unified", null }
            },
            true);
        }

        public override void ImportWorkingCopyDiff(string path, string diffPath)
        {
            TortoiseGitMerge(new Dictionary<string, string>
            {
                { "patchpath", path },
                { "diff", diffPath },
            },
            true);
        }

        private void TortoiseGitProc(string command, Dictionary<string, string> arguments = null, bool waitForExit = false)
        {
            arguments = arguments ?? new Dictionary<string, string>();
            var argumentsString = $"/command:{command} {string.Join(" ", arguments.Select(kv => kv.Value != null ? $"/{kv.Key}:{kv.Value}" : $"/{kv.Key}"))}";
            var processStartInfo = new ProcessStartInfo("TortoiseGitProc.exe", argumentsString);
            ProcessLauncher.Launch(processStartInfo, waitForExit);
        }

        private void TortoiseGitMerge(Dictionary<string, string> arguments = null, bool waitForExit = false)
        {
            arguments = arguments ?? new Dictionary<string, string>();
            var argumentsString = $"{string.Join(" ", arguments.Select(kv => kv.Value != null ? $"/{kv.Key}:{kv.Value}" : $"/{kv.Key}"))}";
            var processStartInfo = new ProcessStartInfo("TortoiseGitMerge.exe", argumentsString);
            ProcessLauncher.Launch(processStartInfo, waitForExit);
        }
    }
}
