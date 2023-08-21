using UnityEngine;
using UnityEditor;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;

public class ExcelWriterHelper : MonoBehaviour
{
    /// <summary>
    /// 一个excel的写入类
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="worksheetNumber">工作表页码</param>
    /// <param name="columnNumber">列号</param>
    /// <param name="rowNumber">行号</param>
    /// <param name="content">内容</param>
    public static void WriteToCell(string filePath, int worksheetNumber, int columnNumber, int rowNumber, string content)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[worksheetNumber];

            var cell = worksheet.Cells[rowNumber,columnNumber];
            cell.Value = content;

            package.Save();
        }
    }
    /// <summary>
    /// Rusk配置表shark写入
    /// </summary>
    /// <param name="worksheetNumber">工作表页码</param>
    /// <param name="AttriName">属性名称</param>
    /// <param name="rowNumber">行号</param>
    /// <param name="content">内容</param>
    public static void WriteFishSetting(int worksheetNumber, string AttriName,int rowNumber, string content)
    {
        string ExcelPath = PathConverter.GetSettingPathOfRusk();

        int AttributeNum = ExcelReaderHelper. GetAttributeNum(worksheetNumber, AttriName, ExcelPath);

        WriteToCell(ExcelPath, worksheetNumber, AttributeNum, rowNumber, content);
    }



}
