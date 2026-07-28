using System.Collections.Generic;

namespace GC.AssetFramework
{
    [System.Serializable]
    public class ModuleData 
    {
        public string ModuleName;
        public bool IsBuild;
        public PackageData[] Packages;
    }
}

