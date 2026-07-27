using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GC.AssetFramework
{
    public class BundleBehaviour
    {
        /// <summary>
        /// 模块配置列表
        /// </summary>
        protected List<BundleModuleData> moduleDataList;
        
        /// <summary>
        /// 行列表
        /// </summary>
        protected List<List<BundleModuleData>> rowModuleDataList;

        /// <summary>
        /// 目标平台
        /// </summary>
        protected string curPlatform;

        /// <summary>
        /// 初始化文件
        /// </summary>
        public virtual void Initialize()
        {
            moduleDataList = BuildBundleConfigura.Instance.AssetBundleConfg;
            rowModuleDataList = new List<List<BundleModuleData>>();
            for (int i = 0; i < moduleDataList.Count; i ++)
            {
                int rowIndex = Mathf.FloorToInt(i / 5);
                if (rowModuleDataList.Count < rowIndex + 1)
                {
                    rowModuleDataList.Add(new List<BundleModuleData>());
                }
                rowModuleDataList[rowIndex].Add(moduleDataList[i]);
            }
#if UNITY_IOS
    curPlatform = "BuildSettings.iPhone";
#else
    curPlatform = "BuildSettings.Android";
#endif
        }

        /// <summary>
        /// 渲染Item
        /// </summary>
        [OnInspectorGUI]
        public virtual void OnGUI()
        {
            if (rowModuleDataList == null)
            {
                return;
            }
            GUIContent content = EditorGUIUtility.IconContent("SceneAsset Icon".Trim(), "测试文字显示");
            content.tooltip = "单击可选中和取消\n快速双击可打开配置窗口";
            for (int i = 0; i < rowModuleDataList.Count; i ++)
            {
                GUILayout.BeginHorizontal();
                for (int j = 0; j < rowModuleDataList[i].Count; j ++)
                {
                    BundleModuleData moduleData = rowModuleDataList[i][j];
                    
                    if (GUILayout.Button(content, GUILayout.Width(130), GUILayout.Height(170)))
                    {
                        moduleData.isBuild = !moduleData.isBuild ? true : false;
                        if (Time.realtimeSinceStartup-moduleData.LastClickButtonime <= 0.18f)
                        {
                            BundleModuleConfig.ShowWindow(moduleData.moduleName);
                        }
                        moduleData.LastClickButtonime = Time.realtimeSinceStartup;
                    }
                    Rect buttonRect = GUILayoutUtility.GetLastRect();
                    Rect labelRect = new Rect(buttonRect.x, buttonRect.y + (buttonRect.height / 1.2f), 130, 20);
                    GUI.Label(labelRect, moduleData.moduleName, new GUIStyle{ alignment = TextAnchor.MiddleCenter });
                    if (moduleData.isBuild)
                    {
                        GUIStyle style = UnityEditorUtility.GetGUIStyle("LightmapEditorSelectedHighlight");
                        style.contentOffset = new Vector2(100, -70);
                        GUI.Toggle(new Rect(10 + (j * 133), -160 + 1 * (i + 1) + ((i + 1) * 170), 120, 160), true, EditorGUIUtility.IconContent("Collab"), style);
                    }
                }
                if (i == rowModuleDataList.Count - 1)
                {
                    DrawAddModuleButton();
                }
                GUILayout.EndHorizontal();
            }

            if (rowModuleDataList.Count == 0)
            {
                DrawAddModuleButton();
            }
            DrawBuildButtons();
        }

        /// <summary>
        /// 绘制打包按钮
        /// </summary>
        public virtual void DrawBuildButtons()
        {
            
        }

        /// <summary>
        /// 绘制补丁包打包按钮
        /// </summary>
        public virtual void BuildBundle()
        {
            
        }

        /// <summary>
        /// 添加资源模块
        /// </summary>
        public virtual void DrawAddModuleButton()
        {
            
        }
    }
}
