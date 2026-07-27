using UnityEngine;

namespace GC.AssetFramework
{
    public class UnityEditorUtility
    {
        /// <summary>
        /// 得到Style
        /// </summary>
        /// <param name="styleName"></param>
        /// <returns></returns>
        public static GUIStyle GetGUIStyle(string styleName)
        {
            GUIStyle gUIStyle = null;
            foreach (var item in GUI.skin.customStyles)
            {
                if (string.Equals(item.name.ToLower(), styleName.ToLower()))
                {
                    gUIStyle = item;
                    break;
                }
            }
            return gUIStyle;
        }
    }
}

