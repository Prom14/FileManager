using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private string[] changedFilesInDesktop;
        private string[] drives;
        private string[] Commands =  {"help","createfile","createfolder","deletefile","deletefolder","clear","quit"};
        private string backText;
        private string pathToDesktopDirectory;
        private bool isGet = false;
        private List<string> paths = new List<string>();
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
            BT_Back.IsEnabled = false;
            BT_Next.IsEnabled = false;

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
            BT_Back.IsEnabled = true;
            if (TB_Path.Text == PathToDesktopDirectory)
            {
                if (changedFilesInDesktop == null)
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
                        if (paths.Count != 0)
                        {
                            for (int i = 0; i < paths.Count; i++)
                            {
                                if (paths[i] == TB_Path.Text)
                                {
                                    isGet = true;
                                }
                                else
                                {
                                    paths.Clear();
                                    paths.Add(TB_Path.Text);
                                }

                                if (isGet != true)
                                {
                                    paths.Add(TB_Path.Text);

                                }


                            }
                        }
                        else
                        {
                            paths.Add(TB_Path.Text);
                        }
                    }
                } 
                else
                {
                    if (File.Exists(AllFilesInDesktopDirectory[Convert.ToInt32(name)]))
                    {
                        System.Diagnostics.Process.Start(AllFilesInDesktopDirectory[Convert.ToInt32(name)]);
                    }
                    else
                    {

                        TB_Path.Text = changedFilesInDesktop[Convert.ToInt32(name)];
                        Grid_Desktop.Children.Clear();
                        Grid_Desktop.RowDefinitions.Clear();
                        Grid_Desktop.ColumnDefinitions.Clear();
                        string[] files = work.GetFiles(changedFilesInDesktop[Convert.ToInt32(name)]);
                        string[] folder = work.GetSubfolders(changedFilesInDesktop[Convert.ToInt32(name)]);
                        AllFoldersAndFilesInFolder = folder.Concat(files).ToArray();
                        CreatedFileAndFolderInGroupBox(AllFilesInDesktopDirectory[Convert.ToInt32(name)], AllFoldersAndFilesInFolder, Grid_Desktop);
                        if (paths.Count != 0)
                        {
                            for (int i = 0; i < paths.Count; i++)
                            {
                                if (paths[i] == TB_Path.Text)
                                {
                                    isGet = true;
                                }
                                else
                                {
                                    paths.Clear();
                                    paths.Add(TB_Path.Text);
                                }

                                if (isGet != true)
                                {
                                    paths.Add(TB_Path.Text);

                                }


                            }
                        }
                        else
                        {
                            paths.Add(TB_Path.Text);
                        }
                    }
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
                    CreatedFileAndFolderInGroupBox(TB_Path.Text, AllFoldersAndFilesInFolder, Grid_Desktop);
                    isGet = false;
                    if (paths.Count != 0)
                    {
                        for (int i = 0; i < paths.Count; i++)
                        {
                            if (paths[i] == TB_Path.Text)
                            {
                                isGet = true;
                            }

                            if(paths.Count > 1 && paths[1] != TB_Path.Text)
                            {
                                paths.RemoveAt(1);
                            }
                        }
                        if (isGet != true)
                        {
                            paths.Add(TB_Path.Text);

                        }
                    }
                    else
                    {
                        paths.Add(TB_Path.Text);
                    }
                }
                else
                {
                    try
                    {
                        System.Diagnostics.Process.Start(AllFoldersAndFilesInFolder[Convert.ToInt32(name)]);
                    }
                    catch (Exception j)
                    {
                        Console.Text = "Невозможно запустить программу(недостаточно прав)"  + "/n" + j.ToString(); // выводим об ошибке в консоль
                    }
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
                        string CurPath = TB_Path.Text;
                        string[] files = work.GetFiles(CurPath);
                        string[] folder = work.GetSubfolders(CurPath);
                        if (CurPath == pathToDesktopDirectory)//Если мы на рабочем столе
                        {
                            AllFilesInDesktopDirectory = folder.Concat(files).ToArray();
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(CurPath, AllFilesInDesktopDirectory, Grid_Desktop);
                            changedFilesInDesktop = null;
                        }
                        else//Если мы не на рабочем столе
                        {
                            AllFoldersAndFilesInFolder = folder.Concat(files).ToArray();
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(CurPath, AllFoldersAndFilesInFolder, Grid_Desktop);
                        }
                        
                    }
                    catch (Exception)
                    {
                        //Выводим в консоль: 
                       
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
                        if (currentPath == pathToDesktopDirectory)
                        {
                            changedFilesInDesktop = work.GetAllFilesThenAllFolders(currentPath);
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                        }
                        else
                        {
                            AllFoldersAndFilesInFolder = work.GetAllFilesThenAllFolders(currentPath);
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, AllFoldersAndFilesInFolder, Grid_Desktop);
                        }

                    }
                    catch (Exception)
                    {
                        Console.Text += "Ошибка сортировки! \r\n";
                    }
                }
                if (SelectedIndex == 4)//Сначала папки 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        if (currentPath == pathToDesktopDirectory)
                        {
                            changedFilesInDesktop = work.GetAllFoldersThenAllFiles(currentPath);
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                        }
                        else
                        {
                            AllFoldersAndFilesInFolder = work.GetAllFoldersThenAllFiles(currentPath);
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, AllFoldersAndFilesInFolder, Grid_Desktop);
                        }

                    }
                    catch (Exception)
                    {
                        Console.Text += "Ошибка сортировки! \r\n";
                    }
                }
                if (SelectedIndex == 5)//Только файлы 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        if (currentPath == pathToDesktopDirectory)
                        {
                            changedFilesInDesktop = work.GetFiles(currentPath);
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                        }
                        else
                        {
                            AllFoldersAndFilesInFolder = work.GetFiles(currentPath);
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, AllFoldersAndFilesInFolder, Grid_Desktop);
                        }

                    }
                    catch (Exception)
                    {
                        Console.Text += "Ошибка сортировки! \r\n";
                    }
                }
                if (SelectedIndex == 6)//Только папки 
                {
                    string currentPath = TB_Path.Text;
                    if (currentPath == pathToDesktopDirectory)
                    {
                        changedFilesInDesktop = work.GetSubfolders(currentPath);
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                    }
                    else
                    {
                        AllFoldersAndFilesInFolder = work.GetSubfolders(currentPath);
                        ClearGrid(Grid_Desktop);
                        CreatedFileAndFolderInGroupBox(currentPath, AllFoldersAndFilesInFolder, Grid_Desktop);
                    }

                }
            }
            else
            {
                int countItems = CB_Sort.Items.Count;
                if (countItems == 0)
                {
                    Console.Text += "Не найдено ни одного файла!! \r\n";
                    //LB_emptyListMessage.Visibility = Visibility.Visible; 
                }

                if (SelectedIndex == countItems - 1)//Без расширения 
                {
                    try
                    {
                        string currentPath = TB_Path.Text;
                        string[] NecessaryFiles = { };
                        string extension = "";
                        if (currentPath == pathToDesktopDirectory)
                        {
                            changedFilesInDesktop = work.GetFilesWithNecessaryExtention(currentPath, extension);
                            if (changedFilesInDesktop.Length == 0)
                            {
                                CB_Sort.SelectedIndex = 0;
                                Console.Text += "Не было найдено файлов без расширения!! \r\n";
                                
                            }
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                        }
                        else
                        {
                            AllFilesInDesktopDirectory = work.GetFilesWithNecessaryExtention(currentPath, extension);
                            if (AllFilesInDesktopDirectory.Length == 0)
                            {
                                CB_Sort.SelectedIndex = 0;
                                Console.Text += "Не было найдено файлов без расширения!! \r\n";
                                
                            }
                            ClearGrid(Grid_Desktop);
                            CreatedFileAndFolderInGroupBox(currentPath, AllFilesInDesktopDirectory, Grid_Desktop);
                        }

                    }
                    catch (Exception)
                    {
                        Console.Text += "Ошибка сортировки! \r\n";
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
                            if (currentPath == pathToDesktopDirectory)
                            {
                                changedFilesInDesktop = work.GetFilesWithNecessaryExtention(currentPath, extension);
                                if (changedFilesInDesktop.Length == 0)
                                {
                                    CB_Sort.SelectedIndex = 0;
                                    Console.Text += "Не было найдено файлов без расширения!! \r\n";
                                   
                                }
                            }
                            else
                            {
                                allFoldersAndFilesInFolder = work.GetFilesWithNecessaryExtention(currentPath, extension);
                                if (allFoldersAndFilesInFolder.Length == 0)
                                {
                                    CB_Sort.SelectedIndex = 0;
                                    Console.Text += "Не было найдено файлов без расширения!! \r\n";
                                    
                                }
                            }
                            changedFilesInDesktop = work.GetFilesWithNecessaryExtention(currentPath, extension);
                            ClearGrid(Grid_Desktop);
                            if (currentPath == pathToDesktopDirectory)
                            {
                                CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                            }
                            else
                            {
                                CreatedFileAndFolderInGroupBox(currentPath, allFoldersAndFilesInFolder, Grid_Desktop);
                            }
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
                                CB_Sort.SelectedIndex = 0;
                                //В консоль - не было найдено файлов без расширения! 
                                Console.Text += "Не было найдено файлов без расширения!! \r\n";
                                return;
                            }
                            if (currentPath == pathToDesktopDirectory)
                            {
                                changedFilesInDesktop = NecessaryFiles;
                                CreatedFileAndFolderInGroupBox(currentPath, changedFilesInDesktop, Grid_Desktop);
                            }
                            else
                            {
                                allFoldersAndFilesInFolder = NecessaryFiles;
                                CreatedFileAndFolderInGroupBox(currentPath, allFoldersAndFilesInFolder, Grid_Desktop);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.Text += "Ошибка сортировки! \r\n";
                       
                    }
                }
            }
            try
            {
                Console.SelectionStart = Console.Text.Length;
                Console.Focus();
                backText += Console.Text;
            }
            catch (Exception)
            {

            }
        }

        private void ClearGrid(Grid grid)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
        }

        private void BT_Back_Click(object sender, RoutedEventArgs e)
        {
           if(TB_Path.Text == pathToDesktopDirectory)
           {
                BT_Back.IsEnabled = false;
                BT_Next.IsEnabled = false;
           }
           else
           {
                BT_Next.IsEnabled = true;
                ClearGrid(Grid_Desktop);
                string path = TB_Path.Text;
                int count = 0;
                char[] simvol = path.ToCharArray();
                for(int j = 0; j < simvol.Length; j++)
                {
                    if(simvol[j] == 92)
                    {
                        count++;
                    }
                }
                if(count > 4)
                {
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (paths[i] == path)
                        {
                            string[] files = work.GetFiles(paths[i - 1]);
                            string[] folder = work.GetSubfolders(paths[i - 1]);
                            if (AllFoldersAndFilesInFolder.Length != 0)
                            {
                                Array.Clear(AllFoldersAndFilesInFolder, 0, AllFoldersAndFilesInFolder.Length);
                            }
                            AllFoldersAndFilesInFolder = folder.Concat(files).ToArray();
                            CreatedFileAndFolderInGroupBox(paths[i - 1], AllFoldersAndFilesInFolder, Grid_Desktop);
                            TB_Path.Text = paths[i - 1];
                        }
                    }
                }
                else
                {
                    CreatedFileAndFolderInGroupBox(PathToDesktopDirectory, AllFilesInDesktopDirectory, Grid_Desktop);
                    BT_Back.IsEnabled = false;
                    if (AllFoldersAndFilesInFolder.Length != 0)
                    {
                        Array.Clear(AllFoldersAndFilesInFolder, 0, AllFoldersAndFilesInFolder.Length);
                    }
                    TB_Path.Text = PathToDesktopDirectory;
                }
           }
           CB_Sort.SelectedIndex = 0;
        }

        private void BT_Next_Click(object sender, RoutedEventArgs e)
        {
            string nextPath = "";
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i] == TB_Path.Text)
                {
                    try
                    {
                        nextPath = paths[i + 1];
                        if (TB_Path.Text == paths[paths.Count - 1])
                        {
                            BT_Next.IsEnabled = false;
                        }
                        else
                        {
                            BT_Next.IsEnabled = true;
                        }
                    }
                    catch (Exception)
                    {
                        Console.Text += "Невозможно вернутся дальше.";
                        TB_Console.IsSelected = true;
                        TI_Operation.IsSelected = false;
                    }
                }
                else if (TB_Path.Text == pathToDesktopDirectory)
                {
                    nextPath = paths[0];
                    BT_Next.IsEnabled = true;
                }
                else
                {
                    BT_Next.IsEnabled = false;
                }
            }
            if (nextPath != "")
            {
                ClearGrid(Grid_Desktop);
                string[] files = work.GetFiles(nextPath);
                string[] folder = work.GetSubfolders(nextPath);
                if (AllFoldersAndFilesInFolder.Length != 0)
                {
                    Array.Clear(AllFoldersAndFilesInFolder, 0, AllFoldersAndFilesInFolder.Length);
                }
                AllFoldersAndFilesInFolder = folder.Concat(files).ToArray();
                CreatedFileAndFolderInGroupBox(nextPath, AllFoldersAndFilesInFolder, Grid_Desktop);
                TB_Path.Text = nextPath;
                BT_Back.IsEnabled = true;
            }
            CB_Sort.SelectedIndex = 0;
        }

        private void Console_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                
                string text;
                if(backText != null)
                {
                    text = Console.Text.Remove(0,backText.ToCharArray().Length);
                    backText = text;
                }
                else

                {
                    text = Console.Text;
                }
                string command = "";
                string attr = "";
                int count = 0;
                bool whiteSpace = false; ;
                char[] commands = text.ToCharArray();
                for (int j = 0; j < commands.Length; j++)
                {
                    count++;
                    if (Char.IsWhiteSpace(commands[j])) { whiteSpace = true; break; }

                }
                if (whiteSpace == true)
                {
                    command = text.Remove(count - 1);
                    attr = text.Remove(0, count);
                }
                else
                {
                    command = text;
                }
                #region Commands
                if (command == "help")
                {
                    Console.Text += "\r\n";
                    for (int i = 0; i < Commands.Length; i++)
                    {
                        Console.Text += Commands[i] + " ";
                    }
                    Console.Text += "\r\n";
                }
                else if(command == "quit")
                {
                    Application app = Application.Current;

                    app.Shutdown();
                }
                else if(command == "createfolder")
                {
                    try
                    {
                        if (work.CreateDir(attr))
                        {
                            Console.Text += "\r\n" + "Папка успешно создана по пути" + " " + attr + "\r\n";
                        }
                        else
                        {
                            Console.Text += "\r\n" + "Создание папки ... прошло неуспешно. Причина: папка по такому пути уже есть. \r\n";
                        }
                    }
                    catch (Exception)
                    {
                        Console.Text += "Создание папки ... прошло неуспешно. Причина: невалидный путь." + "\r\n";
                    }
                }
                else if(command == "createfile")
                {
                    try
                    {
                        if (work.CreateFile(attr))
                        {
                            Console.Text += "\r\n" + "Файл успешно создан по пути" + " " + attr + "\r\n";
                        }
                        else
                        {
                            Console.Text += "\r\n" + "Создание файла ... прошло неуспешно. Причина: файл по такому пути уже есть. \r\n";
                        }
                    }
                    catch (Exception)
                    {
                        Console.Text += "Создание папки ... прошло неуспешно. Причина: невалидный путь." + "\r\n";
                    }
                }
                else if(command == "deletefolder")
                {
                    try
                    {
                        if (work.DeleteDir(attr))
                        { 
                            Console.Text += "\r\n" + "Папка успешно удалена по пути" + " " + attr + "\r\n";
                        }
                        else
                        {
                            Console.Text += " \r\n Удаление папки ... прошло неуспешно. Причина: папка не была найдена по пути:" + " " + attr + "\r\n";
                        }
                    }
                    catch (Exception)
                    {
                        Console.Text += "Удаление папки ... прошло неуспешно. Причина: папка не была найдена по пути:" + " " + attr + "\r\n";
                    }
                }
                else if (command == "deletefile")
                {
                    try
                    {
                        if (work.DeleteFile(attr))
                        {
                            Console.Text += "\r\n" + "Файл успешно удален по пути" + " " + attr + "\r\n";
                        }
                        else
                        {
                            Console.Text += " \r\n Удаление файла ... прошло неуспешно. Причина: файл не был найден по пути:" + " " + attr + "\r\n";
                        }

                    }
                    catch (Exception)
                    {
                        Console.Text += "Удаление файла ... прошло неуспешно. Причина: файл не был найден по пути:" + " " + attr + "\r\n";
                    }
                }
                else if(command == "clear")
                {
                    Console.Clear();
                }
                else
                {
                    Console.Text += "\r\n" + "Такой команды не существует!" + "\r\n";
                }
                #endregion
                Console.SelectionStart = Console.Text.Length;
                Console.Focus();
                backText = Console.Text;
            }
        }

        private void TB_Basket_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Grid_Basket.Children.Count == 0)
            {
                string[] files = work.GetFiles("C:\\$Recycle.Bin");
                CreatedFileAndFolderInGroupBox("C:\\$Recycle.Bin", files, Grid_Basket);
            }
        }

        private void TB_Tree_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(Grid_Tree.Children.Count == 0)
            {
                TreeView tree = new TreeView();
                tree.Background = System.Windows.Media.Brushes.Transparent; 
                ColumnDefinition col = new ColumnDefinition();
                Grid_Tree.ColumnDefinitions.Add(col);
                Grid.SetColumn(tree, 0);
                Grid_Tree.Children.Add(tree);

                drives = Environment.GetLogicalDrives();

                for(int i = 0; i < drives.Length; i++)
                {
                    TreeViewItem treeViewItem = new TreeViewItem();
                    treeViewItem.Header =  "Диск " + drives[i].ToString().Remove(1);
                    treeViewItem.Name = "dir" + i.ToString();
                    treeViewItem.Margin = new Thickness(0, 0, 0, 10);
                    treeViewItem.Foreground = System.Windows.Media.Brushes.WhiteSmoke;
                    tree.Items.Add(treeViewItem);
                    treeViewItem.PreviewMouseLeftButtonDown +=  new MouseButtonEventHandler(Show);
                }
            }
        }

        private void Show(object sender, EventArgs e)
        {
            TreeViewItem tvItem = (TreeViewItem)sender;
            string name = ((TreeViewItem)sender).Name;

            if (name.Remove(3) == "dir")
            {
                TB_Path.Text = drives[Convert.ToInt32(name.Remove(0, 3))];
                string[] files = Directory.GetFiles(TB_Path.Text);
                string[] folder = Directory.GetDirectories(TB_Path.Text);
                string[] all = folder.Concat(files).ToArray();

                for (int i = 0; i < all.Length; i++)
                {
                    TreeViewItem treeView = new TreeViewItem();
                    treeView.Header = all[i];
                    treeView.Name = "file" + i;
                    treeView.Foreground = System.Windows.Media.Brushes.WhiteSmoke;
                    treeView.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Show);
                    tvItem.Items.Add(treeView);
                    tvItem.IsExpanded = true;

                }
            }
            else
            {
                TB_Path.Text = tvItem.Header.ToString();
                if (File.Exists(TB_Path.Text))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(TB_Path.Text);
                    }
                    catch(Exception)
                    {
                        //Не удалось запустить файл.
                    }
                }
                else
                {
                    string[] files = Directory.GetFiles(TB_Path.Text);
                    string[] folder = Directory.GetDirectories(TB_Path.Text);
                    string[] all = folder.Concat(files).ToArray();

                    for (int i = 0; i < all.Length; i++)
                    {
                        TreeViewItem treeView = new TreeViewItem();
                        treeView.Header = all[i];
                        treeView.Name = "file" + i;
                        treeView.Foreground = System.Windows.Media.Brushes.WhiteSmoke;
                        treeView.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Show);
                        tvItem.Items.Add(treeView);
                        tvItem.IsExpanded = true;

                    }
                }
            }
        }
    }
}
