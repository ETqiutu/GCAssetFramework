using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;
using UnityEditor;
using System;

namespace GCAssetFramework
{
    public class BuildWindow : OdinMenuEditorWindow
    {
        [SerializeField]
        public BuildBundleWindow buildBundleWindow = new BuildBundleWindow();

        [SerializeField]
        public BuildHotPatchWindow buildHotPatchWindow = new BuildHotPatchWindow();

        [MenuItem("GC/Asset Bundle Editor")]
        public static void ShowAssetBundleWindow()
        {
            BuildWindow buildWindow = GetWindow<BuildWindow>();
            buildWindow.position = GUIHelper.GetEditorWindowRect().AlignCenter(985, 620);
            buildWindow.ForceMenuTreeRebuild();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            buildBundleWindow.Initialize();
            buildHotPatchWindow.Initialize();
            OdinMenuTree menTree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "Build", null, EditorIcons.House },
                { "Build/AssetBundle", buildBundleWindow, EditorIcons.UnityLogo },
                { "Build/HotPatch", buildHotPatchWindow, EditorIcons.UnityLogo },
            };
            return menTree;
        }
    }
}
