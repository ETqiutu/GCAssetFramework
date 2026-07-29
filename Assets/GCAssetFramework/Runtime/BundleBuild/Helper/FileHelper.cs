using System.IO;
using UnityEngine;

namespace GC.AssetFramework
{
    public class FileHelper
    {
        /// <summary>
        /// 删除文件夹以及其下所有文件
        /// </summary>
        /// <param name="folderPath"></param> 
        public static void DeletFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                string[] file = Directory.GetFiles(folderPath, "*");
                foreach (var path in file)
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
                Directory.Delete(folderPath);
            }
        }
    }
}
