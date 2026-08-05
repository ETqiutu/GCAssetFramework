namespace GC.AssetsFramework
{
    [System.Serializable]
    public class BundleData
    {
        public string BundleName;

        [Sirenix.OdinInspector.FilePath]
        public string[] AssetsPath;
    }
}
