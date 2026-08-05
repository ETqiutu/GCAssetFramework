#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.Linq;

namespace GC.AssetsFramework
{
    public class BuildBundleWindow : OdinEditorWindow
    {
        [MenuItem("GC/Bundle Builder")]
        private static void OpenWindow()
        {
            var window = GetWindow<BuildBundleWindow>();
            window.titleContent = new GUIContent("Bundle Builder");
            window.Show();
        }

        [BoxGroup("Package Configuration")]
        [LabelText("Package Data")]
        [LabelWidth(100)]
        [OnValueChanged("OnPackageDataChanged")]
        public PackageData packageData;

        [BoxGroup("Package")]
        [LabelText("Module Datas")]
        [LabelWidth(100)]
        public List<ModuleData> Modules = new List<ModuleData>();


        [BoxGroup("Package Configuration")]
        [LabelText("Build Setting")]
        [LabelWidth(100)]
        [OnValueChanged("OnBuildSettingChanged")]
        public BuildSetting buildSetting;

        [BoxGroup("Package Configuration")]
        [LabelText("Version")]
        [LabelWidth(100)]
        public string packageVersion;

        [BoxGroup("Build Settings")]
        [LabelText("Build Target")]
        [LabelWidth(100)]
        public BuildPlatform buildTarget = BuildPlatform.Windows;

        [BoxGroup("Build Settings")]
        [LabelText("Build Option")]
        [LabelWidth(100)]
        public BuildOption buildOption = BuildOption.ChunkBasedCompression;

        [BoxGroup("Build Settings")]
        [LabelText("Is Encrypt")]
        [LabelWidth(100)]
        public bool IsEncrypt;

        [BoxGroup("Build Settings")]
        [LabelText("Encrypt key")]
        [LabelWidth(100)]
        public string EncrypKey;

        [BoxGroup("Build Settings")]
        [LabelText("URL")]
        [LabelWidth(100)]
        public string URL;

        [BoxGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.3f, 0.8f, 0.3f)]
        [LabelText("Build All Bundles")]
        public void BuildBundles()
        {
            foreach (var item in packageData.ModuleDatas)
            {
                if (!item.IsActive) continue;
                AssetBuilder.Initialize(item, buildSetting);
                AssetBuilder.Build();
            }
        }

        [BoxGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.8f, 0.3f, 0.3f)]
        [LabelText("Embedded")]
        public void EmbeddedAllModules()
        {
            foreach (var item in packageData.ModuleDatas)
            {
                if (!item.IsActive) continue;
                AssetBuilder.CopyBundleToStreamingAssets(item);
            }
        }

        [BoxGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.6f, 0.9f)]
        [LabelText("Hot Update")]
        public void HotUpdate()
        {
            
        }

        [BoxGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.6f, 0.1f)]
        [LabelText("Upload Assets")]
        public void UploadAsset()
        {
            
        }

        [BoxGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.9f, 0.7f, 0.2f)]
        [LabelText("Refresh")]
        public void RefreshData()
        {
            packageData.Version = packageVersion;
            packageData.ModuleDatas = Modules.ToArray();
            buildSetting.URL = URL;
            buildSetting.BuildPlatform = buildTarget;
            buildSetting.BuildOption = buildOption;
            buildSetting.EncrypKey = EncrypKey;
            buildSetting.IsEncryption = IsEncrypt;
        }

        private void OnPackageDataChanged()
        {            
            if (packageData != null)
            {
                packageVersion = packageData.Version;
                Modules = packageData.ModuleDatas.ToList();
            }
        }

        private void OnBuildSettingChanged()
        {
            if (buildSetting != null)
            {
                URL = buildSetting.URL;
                buildTarget = buildSetting.BuildPlatform;
                buildOption = buildSetting.BuildOption;
                EncrypKey = buildSetting.EncrypKey;
                IsEncrypt = buildSetting.IsEncryption;
            }
        }
    }
}
#endif