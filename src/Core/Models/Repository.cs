using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AstralKeks.SourceControl.Models
{
    public class Repository
    {
        public Repository(string name, VersionSystem vcs, Uri uri)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            Name = name;
            Vcs = vcs;
            Uri = uri;
        }

        public string Name { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VersionSystem Vcs { get; }

        public Uri Uri { get; }
    }
}
