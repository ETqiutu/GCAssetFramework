using UnityEditor;
using UnityEngine;

namespace GC.AssetFramework
{
    public class BuildHotPatchWindow : BundleBehaviour
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        /// <value></value>
        protected string[] buildButtonsNameArr = new string[]
        {
            "打包热更",
            "上传资源"  
        };

        /// <summary>
        /// 热更描述
        /// </summary>
        [HideInInspector] public string pathDes = "输入本子热更描述";

        /// <summary>
        /// 热更版本
        /// </summary>
        [HideInInspector] public string hotVersion = "1.0.0";

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// 绘制添加资源模块的按钮
        /// </summary>
        public override void DrawAddModuleButton()
        {
            base.DrawAddModuleButton();
            GUIContent addContent = EditorGUIUtility.IconContent("CollabCreate Icon".Trim(), "");
            if (GUILayout.Button(addContent, GUILayout.Width(130), GUILayout.Height(170)))
            {
                BundleModuleConfig.ShowWindow("New Asset Module");
            }
        }

        /// <summary>
        /// 渲染构建按钮
        /// </summary>
        public override void DrawBuildButtons()
        {
            base.DrawBuildButtons();
            GUILayout.BeginArea(new Rect(670, 0, 800, 600));
            GUILayout.BeginVertical();

            for (int i = 0;  i < buildButtonsNameArr.Length; i ++)
            {
                GUIStyle style = UnityEditorUtility.GetGUIStyle("PreButtonBlue");
                style.fixedWidth = 130;
                style.fixedHeight = 100;
                if (GUILayout.Button(buildButtonsNameArr[i], style, GUILayout.Height(100)))
                {
                    if (i == 0)
                    {
                        BuildBundle();
                    }
                    else
                    {
                        CopyBundleToStreamingAssetsPath();
                    }
                }
            }
            DrawInfo();
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        public void DrawInfo()
        {
            GUILayout.Space(80);
            EditorGUILayout.LabelField("版本");
            hotVersion = GUILayout.TextField(hotVersion, GUILayout.Width(130), GUILayout.Height(24));
            GUILayout.Space(10);
            EditorGUILayout.LabelField("请输入本次热更公告");
            pathDes = GUILayout.TextField(pathDes, GUILayout.Width(130), GUILayout.Height(300));
        }

        /// <summary>
        /// 打包按钮
        /// </summary>
        public override void BuildBundle()
        {
            base.BuildBundle();
            string[] v = hotVersion.Split(".");
            int version = int.Parse(v[0] ?? "0") * 100 + int.Parse(v[1] ?? "0") * 10 + int.Parse(v[2] ?? "0");
            foreach (var item in moduleDataList)
            {
                if (item.isBuild)
                {
                    BuildBundleCompiler.BuildAssetBundle(item, BuildType.HotPatch, version, pathDes);
                }
            }
        }

        /// <summary>
        /// 内嵌打包安检触发事件
        /// </summary>
        public void CopyBundleToStreamingAssetsPath()
        {
            foreach (var item in moduleDataList)
            {
                if (item.isBuild)
                {
                    // TODO: 打包
                }
            }
        }
    }
}
