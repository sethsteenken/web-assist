using System;
using System.IO;
using System.Web;

namespace WebAssist.Optimizations
{
    internal abstract class BundleConfigFile
    {
        public BundleConfigFile(string path)
        {
            Path = HttpContext.Current.Server.MapPath(path);
            LastUpdatedDate = File.GetLastWriteTime(Path);
        }

        public string Path { get; protected set; }
        public DateTime LastUpdatedDate { get; set; }

        public BundleConfigFileInfo Content { get; set; }

        public abstract void UpdateContentFromConfiguration();

        protected virtual void CleanBundleInfo()
        {
            if (Content != null)
            {
                if (Content.Scripts != null)
                {
                    foreach (var bundle in Content.Scripts)
                    {
                        if (string.IsNullOrWhiteSpace(bundle.Filename))
                            bundle.Filename = string.Concat(bundle.Name, ".min.js");
                    }
                }

                if (Content.Styles != null)
                {
                    foreach (var bundle in Content.Styles)
                    {
                        if (string.IsNullOrWhiteSpace(bundle.Filename))
                            bundle.Filename = string.Concat(bundle.Name, ".min.css");
                    }
                }
            }
        }
    }
}
