using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System;

namespace GC.AssetFramework
{
    public class BundleSettingWindow : OdinMenuEditorWindow
    {
        [MenuItem("GC/AssetFramework/Bundle Setting")]
        private static void OpenWindow()
        {
            var window = GetWindow<BundleSettingWindow>();
            window.titleContent = new GUIContent("Build Setting");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 400);
            window.ForceMenuTreeRebuild();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree menuTree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "Setting/Bundle Setting", BundleSettings.Instance, EditorIcons.SettingsCog }
            };
            return menuTree;
        }
    }
}
