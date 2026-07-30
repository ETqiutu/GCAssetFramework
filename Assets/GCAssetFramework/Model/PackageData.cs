using UnityEngine;

namespace GC.AssetsFramework
{
    [CreateAssetMenu(fileName = "PackageConfig", menuName = "GC/Assets/Package Data")]
    public class PackageData : ScriptableObject
    {
        public string Version;
        public ModuleData[] ModuleDatas;
    }
}
