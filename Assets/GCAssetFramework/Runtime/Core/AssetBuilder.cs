#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GC.AssetsFramework
{
    public class AssetBuilder
    {
        public static PackageData PackageData = AssetDatabase.LoadAssetAtPath<PackageData>("Assets/GCAssetFramework/Data/PackageConfig.asset");
        public static BuildSetting BuildSetting = AssetDatabase.LoadAssetAtPath<BuildSetting>("Assets/GCAssetFramework/Data/BuildSetting.asset");
        public static string Output = "";

        public static void BuildPackage()
        {
            string platformName = BuildSetting.BuildPlatform.ToString().ToLower();
            Output = Application.dataPath + "/../AssetBundle/" + platformName + "/" + PackageData.Version + "/";
            FileHelper.DeletePath(Output);
            
        }
    }
}
#endif