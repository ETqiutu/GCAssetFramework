using UnityEditor;
using UnityEngine;

namespace GC.AssetFramework
{
    public class BuildAssetBundle : MonoBehaviour
    {
        [MenuItem("GC/AssetFramework/Build")]
        private static void Build()
        {
            for (int i = 0; i < BuildConfigura.Instance.AssetsData.Count; i ++)
            {
                if (BuildConfigura.Instance.AssetsData[i].IsBuild)
                {
                    Debug.Log("构建开始！");
                    BuildBundleCompiler.BuildAssetBundle(BuildConfigura.Instance.AssetsData[i]);
                }
            }
        }
    }
}
