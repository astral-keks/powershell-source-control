﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AstralKeks.SourceControl.Core.Data;

namespace AstralKeks.SourceControl.Core.Client
{
    internal class SvnRepositoryClient : RepositoryClient
    {
        public override VersionSystem Vcs => VersionSystem.Svn;

        public override bool IsWorkingCopy(string directory)
        {
            return Directory.Exists(Path.Combine(directory, ".svn"));
        }

        public override void CreateWorkingCopy(string directory, Uri uri)
        {
            TortoiseProc("checkout", new Dictionary<string, string>
            {
                { "path", directory },
                { "url", uri.ToString() }
            });
        }

        public override void PopWorkingCopy(string path)
        {
            TortoiseProc("update", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void PushWorkingCopy(string path)
        {
            TortoiseProc("commit", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ResetWorkingCopy(string path)
        {
            TortoiseProc("revert", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ShowWorkingCopyDiff(string path)
        {
            TortoiseProc("diff", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ShowWorkingCopyLog(string path)
        {
            TortoiseProc("log", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void AddWorkingCopyItem(string path)
        {
            TortoiseProc("add", new Dictionary<string, string>
            {
                { "path", path }
            });
        }

        public override void ExportWorkingCopyDiff(string path, string diffPath)
        {
            TortoiseProc("createpatch", new Dictionary<string, string>
            {
                { "path", path },
                { "savepath", diffPath },
                { "noview", null },
            },
            true);
        }

        public override void ImportWorkingCopyDiff(string path, string diffPath)
        {
            TortoiseMerge(new Dictionary<string, string>
            {
                { "patchpath", path },
                { "diff", diffPath },
            },
            true);
        }

        private void TortoiseProc(string command, Dictionary<string, string> arguments = null, bool waitForExit = false)
        {
            arguments = arguments ?? new Dictionary<string, string>();
            var argumentsString = $"/command:{command} {string.Join(" ", arguments.Select(kv => $"/{kv.Key}:{kv.Value}"))}";
            var process = Process.Start("TortoiseProc.exe", argumentsString);
            if (process != null && waitForExit)
                process.WaitForExit();
        }

        private void TortoiseMerge(Dictionary<string, string> arguments = null, bool waitForExit = false)
        {
            arguments = arguments ?? new Dictionary<string, string>();
            var argumentsString = $"{string.Join(" ", arguments.Select(kv => kv.Value != null ? $"/{kv.Key}:{kv.Value}" : $"/{kv.Key}"))}";
            var process = Process.Start("TortoiseMerge.exe", argumentsString);
            if (process != null && waitForExit)
                process.WaitForExit();
        }
    }
}
