using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace GC.AssetFramework
{
    [System.Serializable]
    public class PackageData
    {
        public string ModuleName;
        public string PackageName;
        [FilePath] public string[] AssetsPath;
        [FilePath] public string[] Prefabs;
    }
}
