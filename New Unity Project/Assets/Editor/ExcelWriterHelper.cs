using UnityEngine;
using UnityEditor;
using LicenseContext = OfficeOpenXml.LicenseContext;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;

public class ExcelWriterHelper : MonoBehaviour
{
    /// <summary>
    /// һ��excel��д����
    /// </summary>
    /// <param name="filePath">�ļ�·��</param>
    /// <param name="worksheetNumber">������ҳ��</param>
    /// <param name="columnNumber">�к�</param>
    /// <param name="rowNumber">�к�</param>
    /// <param name="content">����</param>
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
    /// Rusk���ñ�sharkд��
    /// </summary>
    /// <param name="worksheetNumber">������ҳ��</param>
    /// <param name="AttriName">��������</param>
    /// <param name="rowNumber">�к�</param>
    /// <param name="content">����</param>
    public static void WriteFishSetting(int worksheetNumber, string AttriName,int rowNumber, string content)
    {
        string ExcelPath = PathConverter.GetSettingPathOfRusk();

        int AttributeNum = ExcelReaderHelper. GetAttributeNum(worksheetNumber, AttriName, ExcelPath);

        WriteToCell(ExcelPath, worksheetNumber, AttributeNum, rowNumber, content);
    }



}
