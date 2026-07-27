using UnityEditor;
using UnityEngine;

namespace GC.AssetFramework
{
    public class BuildBundleWindow : BundleBehaviour
    {
        /// <summary>
        /// 按钮名称
        /// </summary>
        /// <value></value>
        protected string[] buildButtonsNameArr = new string[]
        {
            "打包资源",
            "内嵌资源"  
        };

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
                style.fixedHeight = 300;
                if (GUILayout.Button(buildButtonsNameArr[i], style, GUILayout.Height(300)))
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
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        /// <summary>
        /// 打包按钮
        /// </summary>
        public override void BuildBundle()
        {
            base.BuildBundle();
            foreach (var item in moduleDataList)
            {
                if (item.isBuild)
                {
                    BuildBundleCompiler.BuildAssetBundle(item);
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
