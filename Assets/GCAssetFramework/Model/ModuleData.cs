using System.Collections.Generic;

namespace GC.AssetsFramework
{
    [System.Serializable]
    public class ModuleData
    {
        public string ModuleName;

        public BundleData[] BundleList;
 
        public bool IsActive;
    }
}
