//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.Linq;
//using OfficeOpenXml;
//using System.Xml;
//using System.IO;
//using System;
////using ToonyColorsPro.ShaderGenerator;

////我需要做的，做一个读表的工具 
////根据表格的第三行确认返回的数据类型 输入 的是表格张数，和字段名称 ，返回对应字段的内容 




//public class ExcelReaderHelper : MonoBehaviour
//{
//    static public string filePath = "Assets/Editor/test/Nums/Shark.xlsx";

//    [MenuItem("GameObject/测试1111")]
//    public static void Test()
//    {
//        PathConverter.ConvertToDirectoryPath(filePath);
//        List<int> Nums = ExcelReader(4, "模型ID", PathConverter.ConvertToDirectoryPath(filePath)) as List<int> ;
//        Debug.Log(Nums[1]);
//    }
//    public static object ExcelReader( int workSheetNum,string AttriName,string filePath)
//    {
//        //先匹配行号 
//        List<string> check = ReadRow(filePath, 1, workSheetNum);
//        if(check.IndexOf(AttriName) == -1)
//        {
//            Debug.LogError($"在{workSheetNum}页，没找到{AttriName}");
//        }
//        int AttributeNum = check.IndexOf(AttriName) +1;
//        List<string> PreRead = ReadColumn(filePath, AttributeNum,workSheetNum);
//        return GetData(PreRead);
//    }
//    /// <summary>
//    /// 输入查询到的数组，确认根据数组的第三行，确认转化的数据类型
//    /// </summary>
//    /// <param name="dataType"></param>
//    /// <returns></returns>
//    /// <exception cref="ArgumentException"></exception>
//    public static object GetData(List<string> PreStrings)
//    {
//        switch (PreStrings[2])
//        {
            
//            case "int":
//                PreStrings.RemoveRange(0, 3);
//                return ConvertStringListToIntList02(PreStrings);
//            case "int[]":
//                PreStrings.RemoveRange(0, 3);
//                return ConvertStringListToGenericList<int[]>(PreStrings);
//            case "float":
//                PreStrings.RemoveRange(0, 3);
//                return ConvertStringListToFloatList(PreStrings);
//            case "float[]":
//                PreStrings.RemoveRange(0, 3);
//                return ConvertStringListToGenericList<float[]>(PreStrings);
//            default:
//                throw new ArgumentException("Invalid data type.");
//        }
//    }
//    public static void WriteListToExcel(string filePath, int arrangeNum, List<string> stringList)
//    {
//        // 创建新的Excel文件
//        FileInfo file = new FileInfo(filePath);
//        ExcelPackage package = new ExcelPackage(file);

//        // 获取第一个工作表
//        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

//        // 遍历List写入数据到第四列
//        int rowNum = 1;
//        foreach (string str in stringList)
//        {
//            worksheet.Cells[rowNum, arrangeNum].Value = str;
//            rowNum++;
//        }

//        // 保存并关闭Excel文件
//        package.Save();
//        package.Dispose();
//    }
//    /// <summary>
//    /// int[]改成string
//    /// </summary>
//    /// <param name="intList"></param>
//    /// <returns></returns>
//    public static List<string> ConvertIntListToStringList(List<int[]> intList)
//    {
//        List<string> stringList = new List<string>();

//        foreach (int[] intArray in intList)
//        {
//            string str = string.Join(",", intArray);
//            stringList.Add(str);
//        }

//        return stringList;
//    }
//    /// <summary>
//    /// stirng改int[]
//    /// </summary>
//    /// <param name="stringList"></param>
//    /// <returns></returns>
//    public static List<T> ConvertStringListToGenericList<T>(List<string> stringList) where T : class
//    {
//        if (typeof(T) == typeof(int[]) || typeof(T) == typeof(float[]))
//        {
//            List<T> genericList = new List<T>();

//            foreach (string str in stringList)
//            {
//                string[] strArray = str.Split(',');
//                T array = Activator.CreateInstance<T>();

//                for (int i = 0; i < strArray.Length; i++)
//                {
//                    if (typeof(T) == typeof(int[]))
//                    {
//                        int[] intArray = array as int[];
//                        intArray[i] = int.Parse(strArray[i]);
//                    }
//                    else if (typeof(T) == typeof(float[]))
//                    {
//                        float[] floatArray = array as float[];
//                        floatArray[i] = float.Parse(strArray[i]);
//                    }
//                }

//                genericList.Add(array);
//            }

//            return genericList;
//        }
//        else
//        {
//            throw new ArgumentException("Invalid data type.");
//        }
//    }

//    public static List<float[]> ConvertStringListToFloatList(List<string> stringList)
//    {
//        List<float[]> intList = new List<float[]>();

//        foreach (string str in stringList)
//        {
//            string[] strArray = str.Split(',');
//            float[] intArray = new float[strArray.Length];

//            for (int i = 0; i < strArray.Length; i++)
//            {
//                intArray[i] = int.Parse(strArray[i]);
//            }

//            intList.Add(intArray);
//        }

//        return intList;
//    }
//    /// <summary>
//    /// string改int
//    /// </summary>
//    /// <param name="stringList"></param>
//    /// <returns></returns>
//    public static List<int> ConvertStringListToIntList02(List<string> stringList)
//    {
//        List<int> intList = new List<int>();

//        foreach (string str in stringList)
//        {
//            int intValue = int.Parse(str);
//            intList.Add(intValue);
//        }

//        return intList;
//    }


//    /// <summary>
//    /// 读取excel第一页
//    /// </summary>
//    /// <param name="filePath">路径</param>
//    /// <param name="columnNumber">第几列</param>
//    /// <returns></returns>
//    public static List<string> ReadColumn(string filePath, int columnNumber)
//    {
//        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//解决出现的bug
//        List<string> result = new List<string>();
//        using (var package = new ExcelPackage(new FileInfo(filePath)))
//        {

//            var sheet = package.Workbook.Worksheets[0];
//            for (int row = 1; row <= sheet.Dimension.End.Row; row++)
//            {
//                var cell = sheet.Cells[row, columnNumber];
//                if (cell.Value != null)
//                {
//                    result.Add(cell.Value.ToString());
//                }
//            }
//        }
//        return result;
//    }
//    /// <summary>
//    /// 读取excel
//    /// </summary>
//    /// <param name="filePath">路径</param>
//    /// <param name="columnNumber">第几列</param>
//    /// <param name="WorkSheetsNum">第几张表格</param>
//    /// <returns></returns>
//    public static List<string> ReadColumn(string filePath, int columnNumber,int WorkSheetsNum)
//    {
//        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//解决出现的bug
//        List<string> result = new List<string>();
//        using (var package = new ExcelPackage(new FileInfo(filePath)))
//        {

//            var sheet = package.Workbook.Worksheets[WorkSheetsNum];
//            for (int row = 1; row <= sheet.Dimension.End.Row; row++)
//            {
//                var cell = sheet.Cells[row, columnNumber];
//                if (cell.Value != null)
//                {
//                    result.Add(cell.Value.ToString());
//                }
//            }
//        }
//        return result;
//    }
//    public static List<string> ReadRow(string filePath, int rowNumber, int worksheetNumber)
//    {
//       // ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 解决出现的 bug
//        List<string> result = new List<string>();
//        using (var package = new ExcelPackage(new FileInfo(filePath)))
//        {
//            var sheet = package.Workbook.Worksheets[worksheetNumber];
//            for (int col = 1; col <= sheet.Dimension.End.Column; col++)
//            {
//                var cell = sheet.Cells[rowNumber, col];
//                if (cell.Value != null)
//                {
//                    result.Add(cell.Value.ToString());
//                }
//            }
//        }
//        return result;
//    }
//}
