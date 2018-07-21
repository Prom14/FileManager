using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetDirectoriesFromDrivers();
        }

        List<string> listDirectores = new List<string>();
        public void GetDirectoriesFromDrivers()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();//Get drivers

            string[] str1;
            for (int i = 0; i < allDrives.Length; i++)
            {
                try { str1 = Directory.GetDirectories(allDrives[i].ToString());/*Get directories from all drivers*/ }
                catch { continue; }
                for (int j = 0; j < str1.Length; j++)
                {
                    listDirectores.Add(str1[j]);//Add this directories to a list
                }
            }
        }

        public string[] GetSubfolders(string path)
        {
            string[] str1;
            try {
                str1 = Directory.GetDirectories(path);/*Get directories from path folder*/
                for (int i = 0; i < str1.Length; i++)
                {
                    listDirectores.Add(str1[i]);//Add this directories to a list
                }
                return str1;
            }
            catch { MessageBox.Show("Не удалось получить информацию по этому пути."); return null; }
        }

        static string[] EnumFiles(string path)
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
