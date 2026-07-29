using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;

namespace GC.AssetFramework
{
    public class BundleBuildEditorWindow : OdinEditorWindow
    {
        [MenuItem("GC/AssetFramework/Build Asset")]
        private static void OpenWindow()
        {
            var window = GetWindow<BundleBuildEditorWindow>();
            window.titleContent = new GUIContent("Bundle Build Editor");
            window.Show();
        }

        [FoldoutGroup("Asset Bundle", expanded: true)]
        [ReadOnly]
        [ListDrawerSettings(ShowPaging = false, ShowItemCount = false, DraggableItems = false)]
        [LabelText("模块列表")]
        public List<string> Module1DataList = new List<string>();

        [FoldoutGroup("Asset Bundle", expanded: true)]
        [HorizontalGroup("Asset Bundle/Buttons")]
        [Button(ButtonSizes.Large)]
        [LabelText("打包资源")]
        private void PackageAssetBundle()
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

        [FoldoutGroup("Asset Bundle", expanded: true)]
        [HorizontalGroup("Asset Bundle/Buttons")]
        [Button(ButtonSizes.Large)]
        [LabelText("内嵌资源")]
        private void CopyAssetBundleToStreaming()
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

        [FoldoutGroup("Hot Patch", expanded: true)]
        [ReadOnly]
        [ListDrawerSettings(ShowPaging = false, ShowItemCount = false, DraggableItems = false)]
        [LabelText("模块列表")]
        public List<string> Module2DataList = new List<string>();

        [FoldoutGroup("Hot Patch", expanded: true)]
        [HorizontalGroup("Hot Patch/InputFields")]
        [LabelText("版本号")]
        [LabelWidth(50)]
        public string Version = "";

        [FoldoutGroup("Hot Patch", expanded: true)]
        [HorizontalGroup("Hot Patch/InputFields")]
        [LabelText("公告内容")]
        [LabelWidth(50)]
        public string UpdateNotice = "";

        [FoldoutGroup("Hot Patch", expanded: true)]
        [HorizontalGroup("Hot Patch/Buttons")]
        [Button(ButtonSizes.Large)]
        [LabelText("热更打包")]
        private void HotPatchAssetBundle()
        {
            string[] _version = Version.Split(".");
            int version = int.Parse(_version[0]) * 100 + int.Parse(_version[1]) * 10 + int.Parse(_version[2]);
            for (int i = 0; i < BuildConfigura.Instance.AssetsData.Count; i ++)
            {
                if (BuildConfigura.Instance.AssetsData[i].IsBuild)
                {
                    Debug.Log("GC Asset Framework: Build Start!");
                    BuildBundleCompiler.BuildAssetBundle(BuildConfigura.Instance.AssetsData[i], BuildType.HotPatch, version, UpdateNotice);
                }
            }
        }

        [FoldoutGroup("Hot Patch", expanded: true)]
        [HorizontalGroup("Hot Patch/Buttons")]
        [Button(ButtonSizes.Large)]
        [LabelText("资源上传")]
        private void AssetUpload()
        {
            Debug.Log("资源上传: 尽情期待...");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            foreach (var item in BuildConfigura.Instance.AssetsData)
            {
                string name = item.ModuleName;
                Module1DataList.Add(name);
                Module2DataList.Add(name);
            }
        }
    }
}
