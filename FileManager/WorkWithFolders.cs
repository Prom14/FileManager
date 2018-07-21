using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileManager
{
    class WorkWithFolders
    {
        List<string> listDirectores = new List<string>();
        public string[] GetDirectoriesFromDrivers()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();//Get drivers

            string[] str1 = null;
            for (int i = 0; i < allDrives.Length; i++)
            {
                try { str1 = Directory.GetDirectories(allDrives[i].ToString());/*Get directories from all drivers*/ }
                catch { continue; }
                for (int j = 0; j < str1.Length; j++)
                {
                    listDirectores.Add(str1[j]);//Add this directories to a list
                }
            }
            return listDirectores.ToArray();
        }

        public string[] GetSubfolders(string path)
        {
            string[] str1;
            try
            {
                str1 = Directory.GetDirectories(path);/*Get directories from path folder*/
                for (int i = 0; i < str1.Length; i++)
                {
                    listDirectores.Add(str1[i]);//Add this directories to a list
                }
                return str1;
            }
            catch { MessageBox.Show("Не удалось получить информацию по этому пути."); return null; }
        }

        public string[] EnumFiles(string path)
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path));
                /*foreach (var dir in Directory.GetDirectories(path)) 
                    files.AddRange(EnumFiles(dir));  Тут рекурсия для получения всех файлов из подпапок*/
            }
            catch (UnauthorizedAccessException) { }
            return files.ToArray();
        }
    }
}
