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
        private string[] allFoldersAndFilesInFolder;
        private string pathToDesktopDirectory;
        WorkWithFolders work = new WorkWithFolders();
        public string PathToDesktopDirectory
        {
            get
            {
                return pathToDesktopDirectory;
            }
            set
            {
                pathToDesktopDirectory = value;
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
        public string[] AllFoldersAndFilesInFolder
        {
            get
            {
                return allFoldersAndFilesInFolder;
            }
            set
            {
                allFoldersAndFilesInFolder = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.MinHeight = 600;
            this.MinWidth = 600;
            PathToDesktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            FilesInDesktopDirectory = work.GetFiles(PathToDesktopDirectory.ToString());
            FoldersInDesktopDirectory = work.GetSubfolders(PathToDesktopDirectory.ToString());
            AllFilesInDesktopDirectory = FoldersInDesktopDirectory.Concat(FilesInDesktopDirectory).ToArray();
            TB_Path.Text = PathToDesktopDirectory;

        }

        private void CreatedFileAndFolderInGroupBox(string path, string[] files,Grid _grid)
        {
            for (int i = 0; i < files.Length; i++)
            {
                Bitmap bt = WorkWithFolders.GetIconFile(files[i]);
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
                LB_Date.Content = File.GetCreationTime(files[i]);
                LB_Date.HorizontalAlignment = HorizontalAlignment.Center;
                LB_Date.VerticalAlignment = VerticalAlignment.Center;
                LB_Type.HorizontalAlignment = HorizontalAlignment.Center;
                LB_Type.VerticalAlignment = VerticalAlignment.Center;
                LB_Attributes.HorizontalAlignment = HorizontalAlignment.Center;
                LB_Attributes.VerticalAlignment = VerticalAlignment.Center;
                if (File.Exists(files[i].ToString()) && WorkWithFolders.GetExtension(files[i]).ToString() != ".lnk")//.lnk - ярлык
                {
                    LB_Type.Content = WorkWithFolders.GetExtension(files[i]).ToString();
                    if (WorkWithFolders.GetReadOnly(files[i]))
                    {
                        LB_Attributes.Content = "Только чтение";
                    }
                    else
                    {
                        LB_Attributes.Content = "Чтение и запись";
                    }
                }
                else if (File.Exists(files[i].ToString()) && WorkWithFolders.GetExtension(files[i]).ToString() == ".lnk")
                {
                    LB_Type.Content = ".exe (Ярлык)";
                    if (WorkWithFolders.GetReadOnly(files[i]))
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
                grid.ShowGridLines = false;
                _grid.RowDefinitions.Add(rewDef);
                groupBox.Header = files[i].Remove(0, path.Length + 1);
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
                //icon.MouseEnter += (s, e) => Mouse.OverrideCursor = Cursors.Hand;
                //icon.MouseLeave += (s, e) => Mouse.OverrideCursor = Cursors.Arrow;
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
            string path = TB_Path.Text;
            WorkWithFolders work = new WorkWithFolders();
            if (path == PathToDesktopDirectory)
            {
                if (File.Exists(AllFilesInDesktopDirectory[Convert.ToInt32(name)]))
                {
                    System.Diagnostics.Process.Start(AllFilesInDesktopDirectory[Convert.ToInt32(name)]);
                }
                else
                {
                    TB_Path.Text = AllFilesInDesktopDirectory[Convert.ToInt32(name)];
                    Grid_Desktop.Children.Clear();
                    Grid_Desktop.RowDefinitions.Clear();
                    Grid_Desktop.ColumnDefinitions.Clear();
                    string[] files = work.GetFiles(AllFilesInDesktopDirectory[Convert.ToInt32(name)]);
                    string[] folder = work.GetSubfolders(AllFilesInDesktopDirectory[Convert.ToInt32(name)]);
                    AllFoldersAndFilesInFolder = folder.Concat(files).ToArray();
                    CreatedFileAndFolderInGroupBox(AllFilesInDesktopDirectory[Convert.ToInt32(name)], AllFoldersAndFilesInFolder, Grid_Desktop);
                }
            }
            else
            {
                if (!File.Exists(AllFoldersAndFilesInFolder[Convert.ToInt32(name)]))
                {
                    TB_Path.Text = AllFoldersAndFilesInFolder[Convert.ToInt32(name)];
                    ClearGrid(Grid_Desktop);
                    string[] files = work.GetFiles(AllFoldersAndFilesInFolder[Convert.ToInt32(name)]);
                    string[] folder = work.GetSubfolders(AllFoldersAndFilesInFolder[Convert.ToInt32(name)]);
                    if (AllFoldersAndFilesInFolder.Length != 0)
                    {
                        Array.Clear(AllFoldersAndFilesInFolder, 0, AllFoldersAndFilesInFolder.Length);
                    }
                    AllFoldersAndFilesInFolder = folder.Concat(files).ToArray();
                    CreatedFileAndFolderInGroupBox(path, AllFoldersAndFilesInFolder, Grid_Desktop);
                }
                else
                {
                    System.Diagnostics.Process.Start(AllFoldersAndFilesInFolder[Convert.ToInt32(name)]);
                }
            }
        }

        private void Desktop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(Grid_Desktop.Children.Count == 0)
            {
                CreatedFileAndFolderInGroupBox(PathToDesktopDirectory, AllFilesInDesktopDirectory, Grid_Desktop);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string SortType = ((ListBoxItem)CB_Sort.SelectedItem).Content.ToString();
            string[] help = SortType.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);//Разбиваем строку на элементы массива, учитывая пробел
            if (help.Length > 1)
                SortType = help[1];
            else
                SortType = help[0];
            int SelectedIndex = CB_Sort.SelectedIndex;
            if (SelectedIndex < 7)
            {
                if (SelectedIndex == 0)//Выводим все 
                {
                    try
                    {
                        string CurPath = PathToDesktopDirectory;
                        string[] files = work.GetFiles(CurPath);
                        string[] folder = work.GetSubfolders(CurPath);
                        AllFilesInDesktopDirectory = folder.Concat(files).ToArray();
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(CurPath, AllFilesInDesktopDirectory, Grid_Desktop);
                        return;
                    }
                    catch (Exception)
                    {
                        //Выводим в консоль: 
                        //Ошибка сортировки! 
                    }
                }
                if (SelectedIndex == 1)//По алфавиту 
                {

                }
                if (SelectedIndex == 2)//По дате создания 
                {

                }
                if (SelectedIndex == 3)//Сначала файлы 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        AllFilesInDesktopDirectory = work.GetAllFilesThenAllFolders(currentPath);
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                    }
                    catch(Exception)
                    {
                        //В консоль - Ошибка! 
                    }
                }
                if (SelectedIndex == 4)//Сначала папки 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        AllFilesInDesktopDirectory = work.GetAllFoldersThenAllFiles(currentPath);
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                    }
                    catch (Exception)
                    {
                        //В консоль - Ошибка! 
                    }
                }
                if (SelectedIndex == 5)//Только файлы 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        AllFilesInDesktopDirectory = work.GetFiles(currentPath);
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                    }
                    catch (Exception)
                    {
                        //В консоль - Ошибка! 
                    }
                }
                if (SelectedIndex == 6)//Только папки 
                {
                    string currentPath = TB_Path.Text;
                    AllFilesInDesktopDirectory = work.GetSubfolders(currentPath);
                    ClearGrid(Grid_Desktop);
                    CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                }
            }
            else
            {
                int countItems = CB_Sort.Items.Count;
                if (countItems == 0)
                {
                    //В консоль - не найдено ни одного элемента 
                    //LB_emptyListMessage.Visibility = Visibility.Visible; 
                }

                if (SelectedIndex == countItems - 1)//Без расширения 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        string[] NecessaryFiles = { };
                        string extension = "";
                        currentPath = TB_Path.Text;
                        AllFilesInDesktopDirectory = work.GetFilesWithNecessaryExtention(currentPath, extension);
                        if (NecessaryFiles.Length == 0)
                        {
                            //В консоль - не было найдено файлов без расширения! 
                            return;
                        }
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                    }
                    catch (Exception)
                    {
                        //В консоль - Ошибка сортировки! 
                    }
                }
                else//Только разрешения 
                {

                    try
                    {
                        string[] extensionS = SortType.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);//Разбиваем строку на элементы массива, ориентируюясь на '/' 
                        string[] NecessaryFiles = { };
                        string currentPath = "";

                        if (extensionS.Length < 2)//указано 1 расширение 
                        {
                            string extension = SortType;
                            currentPath = TB_Path.Text;
                            AllFilesInDesktopDirectory = work.GetFilesWithNecessaryExtention(currentPath, extension);
                            ClearGrid(Grid_Desktop);
                            if (AllFilesInDesktopDirectory.Length == 0)
                            {
                                //В консоль - не было найдено файлов без расширения! 
                                return;
                            }
                            CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                        }
                        else//Указано несколько расширений 
                        {
                            for (int i = 0; i < extensionS.Length; i++)
                            {
                                string extension = extensionS[i];
                                currentPath = TB_Path.Text;
                                string[] FilesWithCurrentExtention = work.GetFilesWithNecessaryExtention(currentPath, extension);
                                if (FilesWithCurrentExtention.Length == 0)//Ни одного файла с текущим расширением 
                                    continue;
                                NecessaryFiles = NecessaryFiles.Concat(FilesWithCurrentExtention).ToArray();
                            }
                            ClearGrid(Grid_Desktop);
                            if (NecessaryFiles.Length == 0)
                            {
                                //В консоль - не было найдено файлов без расширения! 
                                return;
                            }
                            AllFilesInDesktopDirectory = NecessaryFiles;
                            CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                        }
                    }
                    catch (Exception)
                    {
                        //Выводим в консоль: 
                        //Ошибка сортировки! 
                        return;
                    }
                }
            }
        }

        private void ClearGrid(Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
        }
    }
}
