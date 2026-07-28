using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace GC.AssetFramework
{
    public class BuildConfiguraEditorWindow : OdinEditorWindow
    {
        [MenuItem("GC/AssetFramework/Build Configura Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<BuildConfiguraEditorWindow>();
            window.titleContent = new GUIContent("Build Configura Editor");
            window.Show();
        }

        [LabelText("构建配置")]
        [OnValueChanged("OnConfigChanged")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public BuildConfigura Config;

        [ShowInInspector]
        [HideLabel]
        [TabGroup("Modules", "模块列表")]
        [OnValueChanged("OnModulesChanged")]
        [ListDrawerSettings(
            DraggableItems = true,
            ShowIndexLabels = true,
            ShowPaging = false,
            NumberOfItemsPerPage = 10,
            HideAddButton = false,
            HideRemoveButton = false
        )]
        private List<ModuleData> ModuleList = new List<ModuleData>();

        [ShowInInspector]
        [HideLabel]
        [TabGroup("Module Detail", "模块详情")]
        [OnValueChanged("OnModuleDetailChanged")]
        [ShowIf("HasSelectedModule")]
        private ModuleData SelectedModule;

        [ShowInInspector]
        [HideLabel]
        [TabGroup("Package Detail", "包详情")]
        [OnValueChanged("OnPackageDetailChanged")]
        [ShowIf("HasSelectedPackage")]
        private PackageData SelectedPackage;

        [ShowInInspector]
        [HideLabel]
        [TabGroup("Status", "状态信息")]
        [ReadOnly]
        private string StatusInfo;

        private int selectedModuleIndex = -1;
        private int selectedPackageIndex = -1;

        protected override void OnEnable()
        {
            base.OnEnable();
            LoadConfig();
            UpdateStatusInfo();
        }

        private void LoadConfig()
        {
            if (BuildConfigura.Instance != null)
            {
                Config = BuildConfigura.Instance;
                ModuleList = new List<ModuleData>(Config.AssetsData);
            }
            else
            {
                // 如果配置不存在，创建一个新的
                Config = CreateInstance<BuildConfigura>();
                ModuleList = new List<ModuleData>();
            }
            UpdateStatusInfo();
        }

        private bool HasSelectedModule()
        {
            return selectedModuleIndex >= 0 && selectedModuleIndex < ModuleList.Count;
        }

        private bool HasSelectedPackage()
        {
            return HasSelectedModule() && 
                   selectedPackageIndex >= 0 && 
                   selectedPackageIndex < ModuleList[selectedModuleIndex].Packages.Length;
        }

        private void UpdateStatusInfo()
        {
            StatusInfo = $"模块数: {ModuleList.Count} | 状态: {(Config != null ? "已加载" : "未加载")}";
        }

        private void OnConfigChanged()
        {
            // 配置改变时同步到模块列表
            if (Config != null && Config.AssetsData != null)
            {
                ModuleList = new List<ModuleData>(Config.AssetsData);
            }
            UpdateStatusInfo();
        }

        private void OnModulesChanged()
        {
            if (Config != null)
            {
                // 同步回Config
                Config.AssetsData = new List<ModuleData>(ModuleList);
                
                // 清空选中的模块和包
                if (selectedModuleIndex >= ModuleList.Count)
                {
                    selectedModuleIndex = -1;
                    SelectedModule = null;
                }
                if (selectedPackageIndex >= 0 && selectedModuleIndex >= 0 && 
                    selectedModuleIndex < ModuleList.Count)
                {
                    var module = ModuleList[selectedModuleIndex];
                    if (selectedPackageIndex >= module.Packages.Length)
                    {
                        selectedPackageIndex = -1;
                        SelectedPackage = null;
                    }
                }
                Config.Save();
            }
            UpdateStatusInfo();
        }

        private void OnModuleDetailChanged()
        {
            if (Config != null && SelectedModule != null)
            {
                // 更新模块列表中的对应项
                if (selectedModuleIndex >= 0 && selectedModuleIndex < ModuleList.Count)
                {
                    ModuleList[selectedModuleIndex] = SelectedModule;
                    Config.AssetsData = new List<ModuleData>(ModuleList);
                    Config.Save();
                }
            }
            UpdateStatusInfo();
        }

        private void OnPackageDetailChanged()
        {
            if (Config != null && SelectedPackage != null && HasSelectedModule())
            {
                var module = ModuleList[selectedModuleIndex];
                var packages = module.Packages.ToList();
                if (selectedPackageIndex >= 0 && selectedPackageIndex < packages.Count)
                {
                    packages[selectedPackageIndex] = SelectedPackage;
                    module.Packages = packages.ToArray();
                    ModuleList[selectedModuleIndex] = module;
                    Config.AssetsData = new List<ModuleData>(ModuleList);
                    Config.Save();
                }
            }
            UpdateStatusInfo();
        }

        [TabGroup("Modules", "模块列表")]
        [Button("添加模块", ButtonSizes.Medium)]
        [GUIColor(0.3f, 0.8f, 0.3f)]
        private void AddModule()
        {
            var newModule = new ModuleData
            {
                ModuleName = $"Module_{ModuleList.Count + 1}",
                IsBuild = true,
                Packages = new PackageData[0]
            };
            ModuleList.Add(newModule);
            OnModulesChanged();
            
            // 自动选中新添加的模块
            selectedModuleIndex = ModuleList.Count - 1;
            SelectedModule = ModuleList[selectedModuleIndex];
            UpdateStatusInfo();
        }

        [TabGroup("Modules", "模块列表")]
        [Button("删除选中模块", ButtonSizes.Medium)]
        [GUIColor(0.9f, 0.3f, 0.3f)]
        [ShowIf("HasSelectedModule")]
        private void RemoveSelectedModule()
        {
            if (selectedModuleIndex >= 0 && selectedModuleIndex < ModuleList.Count)
            {
                if (EditorUtility.DisplayDialog("确认删除", 
                    $"确定要删除模块 '{ModuleList[selectedModuleIndex].ModuleName}' 吗？", 
                    "确定", "取消"))
                {
                    ModuleList.RemoveAt(selectedModuleIndex);
                    selectedModuleIndex = -1;
                    SelectedModule = null;
                    selectedPackageIndex = -1;
                    SelectedPackage = null;
                    OnModulesChanged();
                    UpdateStatusInfo();
                }
            }
        }

        [TabGroup("Module Detail", "模块详情")]
        [Button("添加包", ButtonSizes.Medium)]
        [GUIColor(0.3f, 0.8f, 0.8f)]
        [ShowIf("HasSelectedModule")]
        private void AddPackageToSelectedModule()
        {
            if (HasSelectedModule())
            {
                var module = ModuleList[selectedModuleIndex];
                var packages = module.Packages.ToList();
                var newPackage = new PackageData
                {
                    ModuleName = module.ModuleName,
                    PackageName = $"Package_{packages.Count + 1}",
                    AssetsPath = new string[0],
                    Prefabs = new string[0]
                };
                packages.Add(newPackage);
                module.Packages = packages.ToArray();
                ModuleList[selectedModuleIndex] = module;
                OnModuleDetailChanged();
                
                // 自动选中新添加的包
                selectedPackageIndex = packages.Count - 1;
                SelectedPackage = packages[selectedPackageIndex];
                UpdateStatusInfo();
            }
        }

        [TabGroup("Module Detail", "模块详情")]
        [Button("删除选中包", ButtonSizes.Medium)]
        [GUIColor(0.9f, 0.3f, 0.3f)]
        [ShowIf("HasSelectedPackage")]
        private void RemoveSelectedPackage()
        {
            if (HasSelectedModule() && selectedPackageIndex >= 0)
            {
                var module = ModuleList[selectedModuleIndex];
                var packages = module.Packages.ToList();
                
                if (EditorUtility.DisplayDialog("确认删除", 
                    $"确定要删除包 '{packages[selectedPackageIndex].PackageName}' 吗？", 
                    "确定", "取消"))
                {
                    packages.RemoveAt(selectedPackageIndex);
                    module.Packages = packages.ToArray();
                    ModuleList[selectedModuleIndex] = module;
                    selectedPackageIndex = -1;
                    SelectedPackage = null;
                    OnModuleDetailChanged();
                    UpdateStatusInfo();
                }
            }
        }

        [TabGroup("Package Detail", "包详情")]
        [Button("添加资源路径", ButtonSizes.Small)]
        [GUIColor(0.3f, 0.8f, 0.3f)]
        [ShowIf("HasSelectedPackage")]
        private void AddAssetPath()
        {
            if (HasSelectedPackage())
            {
                string path = EditorUtility.OpenFolderPanel("选择资源文件夹", "Assets", "");
                if (!string.IsNullOrEmpty(path))
                {
                    // 转换为相对路径
                    if (path.StartsWith(Application.dataPath))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    
                    var currentPackage = SelectedPackage;
                    var paths = currentPackage.AssetsPath.ToList();
                    paths.Add(path);
                    currentPackage.AssetsPath = paths.ToArray();
                    SelectedPackage = currentPackage;
                    OnPackageDetailChanged();
                    UpdateStatusInfo();
                }
            }
        }

        [TabGroup("Package Detail", "包详情")]
        [Button("添加预制体路径", ButtonSizes.Small)]
        [GUIColor(0.3f, 0.8f, 0.8f)]
        [ShowIf("HasSelectedPackage")]
        private void AddPrefabPath()
        {
            if (HasSelectedPackage())
            {
                string path = EditorUtility.OpenFolderPanel("选择预制体文件夹", "Assets", "");
                if (!string.IsNullOrEmpty(path))
                {
                    if (path.StartsWith(Application.dataPath))
                    {
                        path = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    
                    var currentPackage = SelectedPackage;
                    var paths = currentPackage.Prefabs.ToList();
                    paths.Add(path);
                    currentPackage.Prefabs = paths.ToArray();
                    SelectedPackage = currentPackage;
                    OnPackageDetailChanged();
                    UpdateStatusInfo();
                }
            }
        }

        [TabGroup("Package Detail", "包详情")]
        [Button("刷新", ButtonSizes.Small)]
        [ShowIf("HasSelectedPackage")]
        private void RefreshPackage()
        {
            OnPackageDetailChanged();
            EditorUtility.DisplayDialog("刷新完成", "包信息已刷新", "确定");
        }

        [TabGroup("Package Detail", "包详情")]
        [InfoBox("请从左侧模块列表中选择一个模块，然后在模块详情中选择一个包进行编辑")]
        [ShowIf("!HasSelectedPackage")]
        private void ShowNoPackageSelectedInfo() { }
    }
}