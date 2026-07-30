using UnityEngine;

namespace GC.AssetsFramework
{
    [CreateAssetMenu(fileName = "BuildSetting", menuName = "GC/Assets/BuildSetting")]
    public class BuildSetting : ScriptableObject
    {
        public string URL;
        public bool IsEncryption;
        public string EncrypKey;
        public BuildPlatform BuildPlatform;
        public BuildOption BuildOption;
    }
}
