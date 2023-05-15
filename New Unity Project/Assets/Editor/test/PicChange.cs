using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;

using System.IO;
using System.Text;

using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.SearchService;


public class PicChange : MonoBehaviour
{
   // static string NumsPath = "Assets/Editor/test/Nums/Nums01.xlsx";
    /// <summary>
    /// 
    /// </summary>
    /// 



    [MenuItem("Assets/打印Path")]
    public static string GetPath()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
        string str = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        return str;
    }


    public static void ChangeFile()
    {

        string basePath = Application.dataPath + "/SharkRes/ToonyFish/test/第四海域皮肤";
        RenameSubFolders(basePath);
        Debug.Log(basePath);
        DeleFile(basePath);
        //删除文件夹
        // Directory.Delete("D:/fish1/Client/Assets/SharkRes/ToonyFish/test/海域贴图\\4031-喜之次鱼", true);
    }
    [MenuItem("Assets/美术/皮肤改名/终极修改")]
    public static void ProChange()
    {
        string Path = AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]));
        string path02 = PathConverter.ConvertToDirectoryPath(Path);
        RenameSubFolders(path02);//重命名所有子文件夹，去除所有乱七八糟的符号
        DeleFile(path02);//删除所有选择路径下的空文件夹
        ChangeFile2(path02);//修改这个文件夹下的所有子文件
        ChangeFile3(path02);//根据修改小小鱼的命名
        //ChangeFile4(Path);//把所有图片输出到文件夹
    }
    [MenuItem("Assets/美术/皮肤改名/确认图片没问题，输出图片")]
    public static void ProChange02()
    {
        string Path = AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]));
        ChangeFile4(Path);
    }
    [MenuItem("Assets/修改测试")]
    static void Fun()
    {
        string Path = AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]));

        //string path02 = PathConverter.ConvertToDirectoryPath(Path);

        //string[] chidPath = Directory.GetDirectories(path02);
        Debug.Log(Directory.GetParent(Path));
        //DeleFile(path02);
    }

    /// <summary>
    /// 删除这个文件夹下的所有空子文件夹
    /// </summary>
    /// <param name="path"></param>
    static void DeleFile(string path)
    {
        string[] chidPath = Directory.GetDirectories(path);
        foreach (string child in chidPath)
        {
            string[] files = Directory.GetFiles(child);
            if (files.Length == 0)
            {
                AssetDatabase.DeleteAsset(PathConverter.ConvertToAssetPath(child));
            }
        }
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 规范化命名的方法,先修改文件夹的命名
    /// </summary>
    /// <param name="path"></param>
    static void RenameSubFolders(string path)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        foreach (DirectoryInfo subdir in dir.GetDirectories())
        {
            // Remove all alphanumeric characters and underscores from folder name
            string newName = System.Text.RegularExpressions.Regex.Replace(subdir.Name, "-", "");
            newName = System.Text.RegularExpressions.Regex.Replace(newName, @"\d+", "");
            newName = System.Text.RegularExpressions.Regex.Replace(newName, @"[a-zA-Z]+", "");
            //newName = newName.Remove(newName.Length - 1, 1) + "_fuzhi/";
            // Rename folder
            subdir.MoveTo(System.IO.Path.Combine(subdir.Parent.FullName, newName));

            // Recursively rename subfolders
            RenameSubFolders(subdir.FullName);
        }
        AssetDatabase.Refresh();
    }

    public static void ChangeFile2(string folderPath)
    {
        // 定义需要操作的文件夹路径
        //string folderPath = Application.dataPath + "/SharkRes/ToonyFish/test/海域贴图";

        // 获取指定路径下所有的png文件
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] pngFiles = directoryInfo.GetFiles("*.png", SearchOption.AllDirectories);

        // 遍历所有的png文件，并根据名称进行重命名
        foreach (FileInfo pngFile in pngFiles)
        {
            string oldName = Path.GetFileNameWithoutExtension(pngFile.Name);
            string newName = oldName;
            string fishNum;
            if (oldName.ToLower().Contains("smallfish"))
            {
                newName = "SmallFish";
            }
            else
            {
                fishNum = oldName.Split("_")[0];
                newName = oldName + "_1";
            }

            if (newName != oldName)
            {
                pngFile.MoveTo(Path.Combine(pngFile.DirectoryName, newName + ".png"));
            }
        }
        AssetDatabase.Refresh();
    }

    public static void ChangeFile3(string folderPath)
    {
        // 定义需要操作的文件夹路径
        //string folderPath = Application.dataPath + "/SharkRes/ToonyFish/test/海域贴图";

        // 获取指定路径下所有的png文件
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] pngFiles = directoryInfo.GetFiles("*.png", SearchOption.AllDirectories);



        string path = "Assets/SharkRes/ToonyFish/test/海域贴图";
        string[] subFolderNames = Directory.GetDirectories(path).Select(x => Path.GetFileName(x)).ToArray();
        for (int i = 0; i < subFolderNames.Length; i++)
        {
            directoryInfo = new DirectoryInfo(Path.Combine(path, subFolderNames[i]));
            FileInfo[] Pngs = directoryInfo.GetFiles("*.png", SearchOption.AllDirectories);
            string fishNum = "";
            foreach (FileInfo pngFile in Pngs)
            {
                string oldName = Path.GetFileNameWithoutExtension(pngFile.Name);
                string newName = oldName;

                if (oldName.ToLower().Contains("smallfish"))
                {

                }
                else
                {
                    fishNum = oldName.Split("_")[0];
                    newName = oldName + "_1";
                    break;
                }
                if (newName != oldName)
                {
                    pngFile.MoveTo(Path.Combine(pngFile.DirectoryName, newName + ".png"));
                }
            }
            foreach (FileInfo pngFile in Pngs)
            {
                string oldName = Path.GetFileNameWithoutExtension(pngFile.Name);
                string newName = oldName;

                if (oldName.ToLower().Contains("smallfish"))
                {
                    newName = fishNum + "_0_1";
                }
                if (newName != oldName)
                {
                    pngFile.MoveTo(Path.Combine(pngFile.DirectoryName, newName + ".png"));
                }
            }
        }
        AssetDatabase.Refresh();
    }

    public static void ChangeFile4(string sourcePath)
    {
        // string sourcePath = "Assets/SharkRes/ToonyFish/test/海域贴图";
        string targetPath = Directory.GetParent(sourcePath).ToString() + "/图片输出";
        Directory.CreateDirectory(targetPath);

        // 获取源文件夹下的所有PNG文件
        string[] pngFiles = Directory.GetFiles(sourcePath, "*.png", SearchOption.AllDirectories);

        // 移动PNG文件到目标文件夹
        foreach (string pngFile in pngFiles)
        {
            // 获取文件名（带扩展名）
            string fileName = Path.GetFileName(pngFile);

            // 拼接目标文件路径
            string targetFile = Path.Combine(targetPath, fileName);

            // 移动文件
            File.Move(pngFile, targetFile);
        }

        // 刷新资源面板
        AssetDatabase.Refresh();
    }


    static private string folderPath = "Assets/SharkRes/ToonyFish/test/基础第四海域"; // 替换为您的文件夹路径
    [MenuItem("Assets/美术/皮肤改名/美术改名第二版")]
    static private void ChangeName10001()
    {
        folderPath = GetPath();
        string path =  PathConverter.ConvertToDirectoryPath(folderPath);
        RenameFilesInFolder(path);
    }
    [MenuItem("Assets/美术/皮肤改名/美术改名第二版之皮肤")]
    static private void ChangeName10002()
    {
        folderPath = GetPath();
        string path = PathConverter.ConvertToDirectoryPath(folderPath);
        RenameFilesInFolder(path,1);
    }
    


    static private void RenameFilesInFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            string[] filePaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string directoryName = Path.GetDirectoryName(filePath);

                if (fileName.EndsWith("0") || fileName.ToLower().Contains("small"))
                {
                    string[] filesInDirectory = Directory.GetFiles(directoryName, "*_1*.png");

                    if (filesInDirectory.Length > 0)
                    {
                        string referenceFileName = Path.GetFileNameWithoutExtension(filesInDirectory[0]);
                        string newFileName = referenceFileName.Substring(0, 4) + ".png";
                        string newFilePath = Path.Combine(directoryName, newFileName);

                        File.Move(filePath, newFilePath);
                    }
                }
                else if (fileName.StartsWith("3"))
                {
                    string newFileName = "4" + fileName.Substring(1) + ".png";
                    string newFilePath = Path.Combine(directoryName, newFileName);

                    File.Move(filePath, newFilePath);
                }
                else if (fileName.Length == 4)
                {
                    string newFileName = "3" + fileName.Substring(1) + ".png";
                    string newFilePath = Path.Combine(directoryName, newFileName);

                    File.Move(filePath, newFilePath);
                }
            }

            Debug.Log("文件重命名完成！");
        }
        else
        {
            Debug.LogError("文件夹路径不存在！");
        }
        AssetDatabase.Refresh();

    }
    static private void RenameFilesInFolder(string folderPath, int skinNum)
    {
        if (Directory.Exists(folderPath))
        {
            string[] filePaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string directoryName = Path.GetDirectoryName(filePath);

                if (skinNum == 0)
                {
                    if (fileName.EndsWith("0") || fileName.Contains("Small"))
                    {
                        string[] filesInDirectory = Directory.GetFiles(directoryName, "*_1*.png");

                        if (filesInDirectory.Length > 0)
                        {
                            string referenceFileName = Path.GetFileNameWithoutExtension(filesInDirectory[0]);
                            string newFileName = referenceFileName.Substring(0, 4) + ".png";
                            string newFilePath = Path.Combine(directoryName, newFileName);

                            File.Move(filePath, newFilePath);
                        }
                    }
                    else if (fileName.StartsWith("3"))
                    {
                        string newFileName = "4" + fileName.Substring(1) + ".png";
                        string newFilePath = Path.Combine(directoryName, newFileName);

                        File.Move(filePath, newFilePath);
                    }
                }
                else
                {

                    if (fileName.EndsWith("0") || fileName.ToLower().Contains("small"))
                    {
                        string[] filesInDirectory = Directory.GetFiles(directoryName, "*_1*.png");

                        if (filesInDirectory.Length > 0)
                        {
                            string referenceFileName = Path.GetFileNameWithoutExtension(filesInDirectory[0]);
                            string newFileName = "4"+ referenceFileName.Substring(1, 4) + "0_" + skinNum.ToString() + ".png";
                            string newFilePath = Path.Combine(directoryName, newFileName);

                            File.Move(filePath, newFilePath);
                        }
                    }
                    else if (fileName.StartsWith("3"))
                    {
                        string newFileName = "4" + fileName.Substring(1) + "_" + skinNum.ToString() + ".png";
                        string newFilePath = Path.Combine(directoryName, newFileName);

                        File.Move(filePath, newFilePath);
                    }
                }
            }

            Debug.Log("文件重命名完成！");
        }
        else
        {
            Debug.LogError("文件夹路径不存在！");
        }
        AssetDatabase.Refresh();
    }
}



