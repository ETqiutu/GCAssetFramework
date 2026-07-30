using System.Collections.Generic;

namespace GC.AssetsFramework
{
    [System.Serializable]
    public class ManifestData
    {
        public string Version;
        public string UpdateNotice;
        public List<ModuleManifestData> Modules;
    }

    [System.Serializable]
    public class ModuleManifestData
    {
        public string ModuleName;
        public List<BundleManifestData> Bundles;
    }

    [System.Serializable]
    public class BundleManifestData
    {
        public string ModuleName;
        public long BundleSize;
        public List<string> FilePath;
        public List<string> Dependencies;
    }
}