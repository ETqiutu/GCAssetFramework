using System.Collections.Generic;

namespace GC.AssetsFramework
{
    [System.Serializable]
    public class BundleConfig
    {
        public List<BundleInfo> BundleInfoList;
    }

    [System.Serializable]
    public class BundleInfo
    {
        public string Path;
        
        public uint CRC;

        public string BundleName;

        public string AssetName;

        public List<string> Dependencies;
    }

    [System.Serializable]
    public class BuildBundleInfo
    {
        public string FileName;
        public string MD5;
        public float Size;
    }
}