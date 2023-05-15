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



    [MenuItem("Assets/��ӡPath")]
    public static string GetPath()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
        string str = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        return str;
    }


    public static void ChangeFile()
    {

        string basePath = Application.dataPath + "/SharkRes/ToonyFish/test/���ĺ���Ƥ��";
        RenameSubFolders(basePath);
        Debug.Log(basePath);
        DeleFile(basePath);
        //ɾ���ļ���
        // Directory.Delete("D:/fish1/Client/Assets/SharkRes/ToonyFish/test/������ͼ\\4031-ϲ֮����", true);
    }
    [MenuItem("Assets/����/Ƥ������/�ռ��޸�")]
    public static void ProChange()
    {
        string Path = AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]));
        string path02 = PathConverter.ConvertToDirectoryPath(Path);
        RenameSubFolders(path02);//�������������ļ��У�ȥ���������߰���ķ���
        DeleFile(path02);//ɾ������ѡ��·���µĿ��ļ���
        ChangeFile2(path02);//�޸�����ļ����µ��������ļ�
        ChangeFile3(path02);//�����޸�СС�������
        //ChangeFile4(Path);//������ͼƬ������ļ���
    }
    [MenuItem("Assets/����/Ƥ������/ȷ��ͼƬû���⣬���ͼƬ")]
    public static void ProChange02()
    {
        string Path = AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]));
        ChangeFile4(Path);
    }
    [MenuItem("Assets/�޸Ĳ���")]
    static void Fun()
    {
        string Path = AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]));

        //string path02 = PathConverter.ConvertToDirectoryPath(Path);

        //string[] chidPath = Directory.GetDirectories(path02);
        Debug.Log(Directory.GetParent(Path));
        //DeleFile(path02);
    }

    /// <summary>
    /// ɾ������ļ����µ����п����ļ���
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
    /// �淶�������ķ���,���޸��ļ��е�����
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
        // ������Ҫ�������ļ���·��
        //string folderPath = Application.dataPath + "/SharkRes/ToonyFish/test/������ͼ";

        // ��ȡָ��·�������е�png�ļ�
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] pngFiles = directoryInfo.GetFiles("*.png", SearchOption.AllDirectories);

        // �������е�png�ļ������������ƽ���������
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
        // ������Ҫ�������ļ���·��
        //string folderPath = Application.dataPath + "/SharkRes/ToonyFish/test/������ͼ";

        // ��ȡָ��·�������е�png�ļ�
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] pngFiles = directoryInfo.GetFiles("*.png", SearchOption.AllDirectories);



        string path = "Assets/SharkRes/ToonyFish/test/������ͼ";
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
        // string sourcePath = "Assets/SharkRes/ToonyFish/test/������ͼ";
        string targetPath = Directory.GetParent(sourcePath).ToString() + "/ͼƬ���";
        Directory.CreateDirectory(targetPath);

        // ��ȡԴ�ļ����µ�����PNG�ļ�
        string[] pngFiles = Directory.GetFiles(sourcePath, "*.png", SearchOption.AllDirectories);

        // �ƶ�PNG�ļ���Ŀ���ļ���
        foreach (string pngFile in pngFiles)
        {
            // ��ȡ�ļ���������չ����
            string fileName = Path.GetFileName(pngFile);

            // ƴ��Ŀ���ļ�·��
            string targetFile = Path.Combine(targetPath, fileName);

            // �ƶ��ļ�
            File.Move(pngFile, targetFile);
        }

        // ˢ����Դ���
        AssetDatabase.Refresh();
    }


    static private string folderPath = "Assets/SharkRes/ToonyFish/test/�������ĺ���"; // �滻Ϊ�����ļ���·��
    [MenuItem("Assets/����/Ƥ������/���������ڶ���")]
    static private void ChangeName10001()
    {
        folderPath = GetPath();
        string path =  PathConverter.ConvertToDirectoryPath(folderPath);
        RenameFilesInFolder(path);
    }
    [MenuItem("Assets/����/Ƥ������/���������ڶ���֮Ƥ��")]
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

            Debug.Log("�ļ���������ɣ�");
        }
        else
        {
            Debug.LogError("�ļ���·�������ڣ�");
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

            Debug.Log("�ļ���������ɣ�");
        }
        else
        {
            Debug.LogError("�ļ���·�������ڣ�");
        }
        AssetDatabase.Refresh();
    }
}



