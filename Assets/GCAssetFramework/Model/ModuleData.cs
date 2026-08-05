namespace GC.AssetsFramework
{
    [System.Serializable]
    public class ModuleData
    {
        public string ModuleName;

        public Version Version;

        public BundleData[] BundleList;
 
        public bool IsActive;
    }
}
