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
                    Debug.Log("GC Asset Framework: Build Start!");
                    BuildBundleCompiler.BuildAssetBundle(BuildConfigura.Instance.AssetsData[i]);
                }
            }
        }

        [MenuItem("GC/AssetFramework/Embedded")]
        private static void CopyFile()
        {
            for (int i = 0; i < BuildConfigura.Instance.AssetsData.Count; i ++)
            {
                if (BuildConfigura.Instance.AssetsData[i].IsBuild)
                {
                    Debug.Log("GC Asset Framework: Build Start!");
                    BuildBundleCompiler.CopyBundleToStreamingAssets(BuildConfigura.Instance.AssetsData[i]);
                }
            }
        }

        [MenuItem("GC/AssetFramework/HotPatch")]
        private static void HotPatch()
        {
            for (int i = 0; i < BuildConfigura.Instance.AssetsData.Count; i ++)
            {
                if (BuildConfigura.Instance.AssetsData[i].IsBuild)
                {
                    Debug.Log("GC Asset Framework: Build Start!");
                    BuildBundleCompiler.BuildAssetBundle(BuildConfigura.Instance.AssetsData[i], BuildType.HotPatch);
                }
            }
        }
    }
}
