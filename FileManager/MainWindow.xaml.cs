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
        private string[] filesInDesktopDirectory;
        private string[] foldersInDesktopDirectory;
        private string[] allFilesInDesktopDirectory;
        private string path;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }
        public string[] FilesInDesktopDirectory { get => filesInDesktopDirectory; set => filesInDesktopDirectory = value; }
        public string[] FoldersInDesktopDirectory { get => foldersInDesktopDirectory; set => foldersInDesktopDirectory = value; }
        public string[] AllFilesInDesktopDirectory { get => allFilesInDesktopDirectory; set => allFilesInDesktopDirectory = value; }

        public MainWindow()
        {
            InitializeComponent();
            this.MinHeight = 600;
            this.MinWidth = 600;
            WorkWithFolders work = new WorkWithFolders();
            Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            FilesInDesktopDirectory = work.GetFiles(Path.ToString());
            FoldersInDesktopDirectory = work.GetSubfolders(Path.ToString());
            AllFilesInDesktopDirectory = FoldersInDesktopDirectory.Concat(FilesInDesktopDirectory).ToArray();
            if(AllFilesInDesktopDirectory.Length != 0)
             {
                 CreatedGroupBox();
             }

        }

        private void CreatedGroupBox()
        {  
            for(int i = 0; i < AllFilesInDesktopDirectory.Length; i++)
            {
                RowDefinition rewDef = new RowDefinition();
                RowDefinition rewDef2 = new RowDefinition();
                Label LB_Type = new Label();
                GroupBox groupBox = new GroupBox();
                Grid grid = new Grid();
                if(File.Exists(AllFilesInDesktopDirectory[i].ToString()) && WorkWithFolders.GetExtension(AllFilesInDesktopDirectory[i]).ToString() != ".lnk")
                {
                   LB_Type.Content = WorkWithFolders.GetExtension(AllFilesInDesktopDirectory[i]).ToString();
                }
                else if (File.Exists(AllFilesInDesktopDirectory[i].ToString()) && WorkWithFolders.GetExtension(AllFilesInDesktopDirectory[i]).ToString() == ".lnk")
                {
                    LB_Type.Content = ".exe (Ярлык)";
                }
                else
                {
                    LB_Type.Content = "Папка";
                }
                LB_Type.Foreground = Brushes.White;
                grid.ShowGridLines = true;
                Grid_Desktop.RowDefinitions.Add(rewDef);
                groupBox.Header = AllFilesInDesktopDirectory[i].Remove(0, Path.Length + 1);
                groupBox.MinHeight = 75;
                groupBox.BorderBrush = Brushes.Black;
                groupBox.BorderThickness = new Thickness(0.2);
                groupBox.Margin = new Thickness(0, 0, 10, 0);
                Grid.SetRow(groupBox, i);
                Grid_Desktop.Children.Add(groupBox);
                grid.RowDefinitions.Add(rewDef2);
                for(int j = 0; j < 4; j++)
                {
                    ColumnDefinition colDef = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(colDef);
                }
                groupBox.Content = grid;
                Grid.SetColumn(LB_Type, 0);
                grid.Children.Add(LB_Type);
            }
        }
    }
}
