﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Linq;

namespace FileManager
{
    class WorkWithFolders
    {
        public string[] GetDirectoriesFromDrivers()
        {
            List<string> listDirectores = new List<string>();

            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();//Get drivers

                string[] str1 = null;
                for (int i = 0; i < allDrives.Length; i++)
                {
                    try { str1 = Directory.GetDirectories(allDrives[i].ToString());/*Get directories from all drivers*/ }
                    catch { continue; }
                    for (int j = 0; j < str1.Length; j++)
                    {
                        //listDirectores.Add(str1[j]);//Add this directories to a list
                        listDirectores.Insert(j, str1[j]);
                    }
                }
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Неудалось получить файлы и каталоги из диска ... .
                
            }
            return listDirectores.ToArray();
        }

        public string[] GetSubfolders(string path)
        {
            string[] str1;
            try
            {
                str1 = Directory.GetDirectories(path);/*Get directories from path folder*/
                /*for (int i = 0; i < str1.Length; i++)
                {
                    listDirectores.Add(str1[i]);//Add this directories to a list
                }*/
                return str1;
            }
            catch { //MessageBox.Show("Не удалось получить информацию по этому пути.");
                return null; }
        }

        public string[] GetFiles(string path)
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path));
                /*foreach (var dir in Directory.GetDirectories(path)) 
                    files.AddRange(EnumFiles(dir));  Тут рекурсия для получения всех файлов из подпапок*/
            }
            catch (Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Неудалось получить файлы и каталоги из папки ... . Причина: папка пуста или ее не существует.
            }
            return files.ToArray();
        }

        public string[] GetAllFilesThenAllFolders(string path)
        {
            try
            {
                string[] files = GetFiles(path);//Получаем файлы по пути path 
                string[] folders = GetSubfolders(path);//Получаем папки по пути path
                string[] result = files.Concat(folders).ToArray();

                return result;
            }
            catch (Exception)
            {
                //В консоль - ошибка сортировки
                return null;
            }

        }

        public string[] GetAllFoldersThenAllFiles(string path)
        {

            try
            {
                string[] folders = GetSubfolders(path);//Получаем папки по пути path
                string[] files = GetFiles(path);//Получаем файлы по пути path 
                string[] result = folders.Concat(files).ToArray();

                return result;
            }
            catch (Exception)
            {
                //В консоль - ошибка сортировки
                return null;
            }

        }

        public string[] GetFilesWithNecessaryExtention(string path, string necessaryExtention)
        {
            string[] files = GetFiles(path);//Получаем файлы по пути path 

            List<string> necessaryFiles = new List<string>();//Результатный список 

            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    string fileExtention = GetExtension(files[i]);
                    if (fileExtention.ToLower() == necessaryExtention.ToLower())//Если расширения совпадают 
                    {
                        necessaryFiles.Add(files[i]);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return necessaryFiles.ToArray();
        }

        public void CreateDir(string path, string dirName)
        {
            try
            {
                if(!Directory.Exists(path + "\\" + dirName))
                    Directory.CreateDirectory(path + "\\" + dirName);
                else
                {
                    //Выводим сообщение об ошибке в консоль:
                    //Папка с таким именем уже существует.
                }
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Создание папки ... прошло неуспешно. Причина: невалидный путь.
            }
        }
        public bool CreateDir(string path)
        {
            try
            {
                if (!Directory.Exists(path)) { Directory.CreateDirectory(path); return true; }
                else
                {
                    return false;
                    //Выводим сообщение об ошибке в консоль:
                    //Папка с таким именем уже существует.
                }
            }
            catch (Exception)
            {
                return false;
                //Выводим сообщение об ошибке в консоль:
                //Создание папки ... прошло неуспешно. Причина: невалидный путь.
            }
        }
        public bool CreateFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
        public void DeleteDir(string path, string dirName)
        {
            try
            {
                Directory.Delete(path + "\\" + dirName, true);
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Удаление папки ... прошло неуспешно. Причина: папка не была найдена.
            }
            
        }

        public bool DeleteDir(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void DeleteAllFilesFromDir(string path, string dirName)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path + "\\" + dirName);

                //Delete all files
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Удаление файлов из папки ... прошло неуспешно. Причина: папка не была найдена.
            }

        }

        public void DeleteAllFoldersFromDir(string path, string dirName)
        {
            try
            {
                //Delete all directiories
                string[] str = Directory.GetDirectories(path + "\\" + dirName);
                for (int i = 0; i < str.Length; i++)
                {
                    Directory.Delete(str[i], true);
                }
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Удаление каталогов из папки ... прошло неуспешно. Причина: папка не была найдена.
            }
        }

        public void DeleteAllFilesAndFoldersFromDir(string path, string dirName)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path + "\\" + dirName);

                //Delete all files
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }

                //Delete all directiories
                string[] str = Directory.GetDirectories(path + "\\" + dirName);
                for (int i = 0; i < str.Length; i++)
                {
                    Directory.Delete(str[i], true);
                }
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Удаление всех каталогов и файлов из папки ... прошло неуспешно. Причина: папка не была найдена.
            }
        }

        public void RenameDirectory(string path, string dirName, string newName)
        {
            try
            {
                Directory.Move(path + "\\" + dirName, path + "\\" + newName);
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Переименование папки ... прошло неуспешно. Причина: папка не была найдена или введено некорректное название.
            }
        }

        public void RenameFile(string path, string dirName, string fileName, string newName, string extension)
        {
            /*
             КАК ВЫЗЫВАТЬ ФУНКЦИЮ:
                        string extension = str.Substring(str.LastIndexOf('.'));   - тут расщирение файла
                        work.RenameFile(path, dirName, fileName, newName, extension);
             */
            try
            {
                File.Move(path + "\\" + dirName + "\\" + fileName, path + "\\" + dirName + "\\" + newName + extension);
            }
            catch (Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Переименование файла ... по пути ... прошло неуспешно. Причина: файл не был найден или введено некорректное название.
            }
        }

        public void DeleteAllFilesWithСertainExtensionFromDir(string path, string dirName, string extension)
        {
            try
            {
                int CountFiles = 0;
                DirectoryInfo dirInfo = new DirectoryInfo(path + "\\" + dirName);

                //Delete all files
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if(Path.GetExtension(file.Name) == extension)
                    {
                        file.Delete();
                        CountFiles++;
                    }
                }
                if(CountFiles == 0)
                {
                    //В папке ... не было найдено ни одного файла с расширением ... .
                }
            }
            catch (Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Удаление файлов с расширением ... из папки ... прошло неуспешно. Причина: папка не была найдена.
            }
        }

        public void DeleteAllFilesWithСertainExtensionFromDirAndSubForders(string path, string dirName, string extension)
        {
            try
            {
                int CountFiles = 0;
                foreach (var file in Directory.EnumerateFiles(path + "\\" + dirName, "*", SearchOption.AllDirectories))
                {
                    if (Path.GetExtension(file) == extension)
                    {
                        File.Delete(file);
                        CountFiles++;
                    }
                }
                if (CountFiles == 0)
                {
                    //В папке ... не было найдено ни одного файла с расширением ... .
                }
            }
            catch(Exception)
            {
                //Выводим сообщение об ошибке в консоль:
                //Удаление всех файлов с расширением ... из папки ... прошло неуспешно. Причина: папка не была найдена.
            }
        }
        
        public static string GetExtension(string fileName)
        {
            try
            {
                return Path.GetExtension(fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Bitmap GetIconFile(string pathToFile)
        {
            Bitmap bitmap = default(Bitmap);
            try
            {
                bitmap = new Bitmap(System.Drawing.Icon.ExtractAssociatedIcon(pathToFile).ToBitmap());
                return bitmap;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static bool GetReadOnly(string path)//true - only read , false - not only read
        {
            FileAttributes attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }
    }
}
