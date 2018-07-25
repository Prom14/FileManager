using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
        public string[] FilesInDesktopDirectory
        {
            get
            {
                return filesInDesktopDirectory;
            }
            set
            {
                filesInDesktopDirectory = value;
            }
        }
        public string[] FoldersInDesktopDirectory
        {
            get
            {
                return foldersInDesktopDirectory;
            }
            set
            {
                foldersInDesktopDirectory = value;
            }
        }
        public string[] AllFilesInDesktopDirectory
        {
            get
            {
                return allFilesInDesktopDirectory;
            }
            set
            {
                allFilesInDesktopDirectory = value;
            }
        }

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
            for (int i = 0; i < AllFilesInDesktopDirectory.Length; i++)
            {
                Bitmap bt = WorkWithFolders.GetIconFile(AllFilesInDesktopDirectory[i]);
                System.Windows.Controls.Image icon = new System.Windows.Controls.Image();
                if (bt != null)
                {
                    IntPtr hBitmap = bt.GetHbitmap();
                    System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    icon.Source = WpfBitmap;
                    icon.Name = "icon" + i.ToString();
                    icon.Width = 32;
                    icon.Height = 32;
                }
                RowDefinition rewDef = new RowDefinition();
                RowDefinition rewDef2 = new RowDefinition();
                Label LB_Type = new Label();
                Label LB_Date = new Label();
                Label LB_Attributes = new Label();
                GroupBox groupBox = new GroupBox();
                Grid grid = new Grid();
                LB_Date.Content = File.GetCreationTime(AllFilesInDesktopDirectory[i]);
                LB_Date.HorizontalAlignment = HorizontalAlignment.Center;
                LB_Date.VerticalAlignment = VerticalAlignment.Center;
                LB_Type.HorizontalAlignment = HorizontalAlignment.Center;
                LB_Type.VerticalAlignment = VerticalAlignment.Center;
                LB_Attributes.HorizontalAlignment = HorizontalAlignment.Center;
                LB_Attributes.VerticalAlignment = VerticalAlignment.Center;
                if (File.Exists(AllFilesInDesktopDirectory[i].ToString()) && WorkWithFolders.GetExtension(AllFilesInDesktopDirectory[i]).ToString() != ".lnk")//.lnk - ярлык
                {
                    LB_Type.Content = WorkWithFolders.GetExtension(AllFilesInDesktopDirectory[i]).ToString();
                    if (WorkWithFolders.GetReadOnly(AllFilesInDesktopDirectory[i]))
                    {
                        LB_Attributes.Content = "Только чтение";
                    }
                    else
                    {
                        LB_Attributes.Content = "Чтение и запись";
                    }
                }
                else if (File.Exists(AllFilesInDesktopDirectory[i].ToString()) && WorkWithFolders.GetExtension(AllFilesInDesktopDirectory[i]).ToString() == ".lnk")
                {
                    LB_Type.Content = ".exe (Ярлык)";
                    if (WorkWithFolders.GetReadOnly(AllFilesInDesktopDirectory[i]))
                    {
                        LB_Attributes.Content = "Только чтение";
                    }
                    else
                    {
                        LB_Attributes.Content = "Чтение и запись";
                    }
                }
                else
                {
                    var fullFolderPath = @"https://api.icons8.com/download/5d0df72fd1e3f7209d05c11feab5d97faa2730b7/Color/PNG/512/Very_Basic/opened_folder-512.png";
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullFolderPath, UriKind.Absolute);
                    bitmap.EndInit();
                    icon.Source = bitmap;
                    icon.Width = 32;
                    icon.Height = 32;
                    icon.Name = "icon" + i.ToString();
                    LB_Type.Content = "Папка";
                }
  
                LB_Type.Foreground = System.Windows.Media.Brushes.White;
                LB_Date.Foreground = System.Windows.Media.Brushes.White;
                LB_Attributes.Foreground = System.Windows.Media.Brushes.White;
                grid.ShowGridLines = true;
                Grid_Desktop.RowDefinitions.Add(rewDef);
                groupBox.Header = AllFilesInDesktopDirectory[i].Remove(0, Path.Length + 1);
                groupBox.MinHeight = 75;
                groupBox.BorderBrush = System.Windows.Media.Brushes.Black;
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
                icon.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Open);
                icon.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Hand;
                icon.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
                Grid.SetColumn(icon, 0);
                Grid.SetColumn(LB_Type, 1);
                Grid.SetColumn(LB_Date, 2);
                Grid.SetColumn(LB_Attributes, 3);
                grid.Children.Add(icon);
                grid.Children.Add(LB_Type);
                grid.Children.Add(LB_Date);
                grid.Children.Add(LB_Attributes);
            }
        }

        private void Open(object sender, EventArgs e)
        {
            string name = ((System.Windows.Controls.Image)sender).Name.Remove(0,4);
            if (File.Exists(AllFilesInDesktopDirectory[Convert.ToInt32(name)]))
            {
                try
                {
                    System.Diagnostics.Process.Start(AllFilesInDesktopDirectory[Convert.ToInt32(name)]);
                }
                catch (Exception j)
                {
                    MessageBox.Show(j.ToString());
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
