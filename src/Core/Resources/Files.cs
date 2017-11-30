using System;

namespace AstralKeks.SourceControl.Resources
{
    internal class Files
    {
        public const string RepositoryJson = "Repository.json";

        public static string DiffPatch() => $"{DateTime.UtcNow.ToString("yy-MM-dd-HH-mm-ss-ffff")}.patch";
    }
}
