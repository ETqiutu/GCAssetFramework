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

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void WriteFile(string filePath, byte[] data)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            using (FileStream stream = File.Create(filePath))
            {
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
