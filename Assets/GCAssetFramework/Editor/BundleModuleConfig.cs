using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.VisualScripting;
using UnityEngine;

namespace GCAssetFramework
{
    public class BundleModuleConfig : OdinEditorWindow
    {   
        /// <summary>
        /// 输入资源模块名称
        /// </summary>
        /// <returns></returns>
        [PropertySpace(spaceAfter:5, spaceBefore:5)][Required("请输入资源模块名称")][GUIColor(0.5f, 0.5f, 0.5f)][LabelText("资源模块名称")] public string moduleName;

        /// <summary>
        /// 提示
        /// </summary>
        [ReadOnly][HideLabel][TabGroup("预制体包")][DisplayAsString] public string prefabTabel = "该文件夹下的所有预制体都会单独打成一个AssetBundle";

        /// <summary>
        /// 提示
        /// </summary>
        [ReadOnly][HideLabel][TabGroup("文件夹子包")][DisplayAsString] public string rootFolderSubBundle = "该文件夹下的所有子文件夹都会单独打成一个AssetBundle";

        /// <summary>
        /// 提示
        /// </summary>
        [ReadOnly][HideLabel][TabGroup("单个补丁包")][DisplayAsString] public string signBundle = "指定的文件夹会单独打成一个AssetBundle";

        /// <summary>
        /// 预制体资源路径配置
        /// </summary>
        /// <value></value>
        [FolderPath][TabGroup("预制体包")][LabelText("预制体资源路径配置")] public string[] prefabPathArr = new string[] { };

        /// <summary>
        /// 文件夹子包路径配置
        /// </summary>
        /// <value></value>
        [FolderPath][TabGroup("文件夹子包")][LabelText("文件夹子包路径配置")] public string[] rootFolderPathArr = new string[] { };

        /// <summary>
        /// 单个补丁包路径配置
        /// </summary>
        /// <value></value>
        [TabGroup("单个补丁包")][LabelText("单个补丁包路径配置")] public BundleFileInfo[] signFolderPathArr = new BundleFileInfo[] { };
        
        /// <summary>
        /// 窗口显示
        /// </summary>
        /// <param name="moduleName"></param>
        public static void ShowWindow(string moduleName)
        {
            BundleModuleConfig window = GetWindowWithRect<BundleModuleConfig>(new Rect(0, 0, 600, 600));
            window.Show();

            BundleModuleData moduleData = BuildBundleConfigura.Instance.GetBundleDataByName(moduleName);
            if (moduleData != null)
            {
                window.moduleName = moduleData.moduleName;
                window.prefabPathArr = moduleData.PrefabPathArr;
                window.rootFolderPathArr = moduleData.RootFolderPathArr;
                window.signFolderPathArr = moduleData.SignFolderPathArr;
            }
        }

        /// <summary>
        /// 储存模块资源配置
        /// </summary>
        [OnInspectorGUI]
        public void DrawSaveConfiguraButton()
        {
            GUILayout.BeginArea(new Rect(0, 510, 600, 200));
            if (GUILayout.Button("DeleteConfiguration", GUILayout.Height(47)))
            {
                DeleteConfiguration();
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(0, 555, 600, 200));
            if (GUILayout.Button("SaveConfiguration", GUILayout.Height(47)))
            {
                SaveConfiguration();
            }
            
            GUILayout.EndArea();
        }

        /// <summary>
        /// 删除资源模块配置
        /// </summary>
        public void DeleteConfiguration()
        {
            BuildBundleConfigura.Instance.RemoveBundleModuleByName(moduleName);
            UnityEditor.EditorUtility.DisplayDialog("删除成功", "配置已删除", "确定");
            Close();
            BuildWindow.ShowAssetBundleWindow();
        }
        
        /// <summary>
        /// 存储资源模块配置
        /// </summary>
        public void SaveConfiguration()
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                UnityEditor.EditorUtility.DisplayDialog("保存失败!", "模块名称不能为空", "确定");
                return;
            }
            BundleModuleData moduleData = BuildBundleConfigura.Instance.GetBundleDataByName(moduleName);
            if (moduleData == null)
            {
                moduleData = new BundleModuleData();
                moduleData.moduleName = this.moduleName;
                moduleData.PrefabPathArr = prefabPathArr;
                moduleData.RootFolderPathArr = rootFolderPathArr;
                moduleData.SignFolderPathArr = signFolderPathArr;
                moduleData.LastClickButtonime = 0;
                moduleData.isBuild = false;
                BuildBundleConfigura.Instance.SaveModuleData(moduleData);
            }
            else
            {
                moduleData.moduleName = this.moduleName;
                moduleData.PrefabPathArr = prefabPathArr;
                moduleData.RootFolderPathArr = rootFolderPathArr;
                moduleData.SignFolderPathArr = signFolderPathArr;
            }
            UnityEditor.EditorUtility.DisplayDialog("保存成功!", "配置已存储", "确定");
            Close();
            BuildWindow.ShowAssetBundleWindow();
        }
    }
}
