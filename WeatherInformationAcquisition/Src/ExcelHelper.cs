using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.IO;

namespace WeatherInformationAcquisition.Src
{
    public class ExcelHelper
    {
        public static Tuple<string, string[]> ReadSpreadsheetDocument(string docName, string cellName, string sheetName = "Sheet1")
        {
            return GetColumnHeadingAndData(docName, sheetName, cellName);
        }
        public static Tuple<string, string[]> ReadSpreadsheetDocumentFromStream(Stream stream, string cellName, string sheetName = "Sheet1")
        {
            return GetColumnHeadingAndDataFromStream(stream, sheetName, cellName);
        }

        /// <summary>
        /// 使用正则表达式匹配工作簿中单元名称的列名部分
        /// </summary>
        /// <param name="cellName">列名</param>
        /// <returns>匹配到的列名</returns>
        public static string GetColumnName(string cellName)
        {
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellName);

            return match.Value;
        }

        /// <summary>
        /// 将单元格名称作为参数，使用正则表达式匹配工作簿中的单元格名称的行索引部分来获取行索引。
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static uint GetRowIndex(string cellName)
        {
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(cellName);

            return uint.Parse(match.Value);
        }

        /// <summary>
        /// 根据文件地址、工作表名以及单元格名字获取列名和单元格的数据
        /// </summary>
        /// <param name="docName">文件地址</param>
        /// <param name="worksheetName">工作表名</param>
        /// <param name="cellName">单元格名</param>
        /// <returns>列名</returns>
        public static Tuple<string, string[]> GetColumnHeadingAndData(string docName, string worksheetName, string cellName)
        {
            string heading;
            string[] dataArray;
            //打开文件
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(docName, false))
            {
                IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().
                    Where(s => s.Name == worksheetName);
                if (sheets.Count() == 0)
                {
                    return null;
                }
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);

                //获取特定单元格的列名
                string columnName = GetColumnName(cellName);

                //根据列名获取单元格的行数据（按照行序号升序排序）
                IEnumerable<Cell> cells = worksheetPart.Worksheet.Descendants<Cell>().
                    Where(c => string.Compare(GetColumnName(c.CellReference.Value), columnName, true) == 0).
                    OrderBy(r => GetRowIndex(r.CellReference));

                if (cells.Count() == 0)
                {
                    return null;
                }

                Cell headCell = cells.First();

                if (headCell.DataType != null && headCell.DataType.Value == CellValues.SharedString)
                {
                    SharedStringTablePart shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                    heading = items[int.Parse(headCell.CellValue.Text)].InnerText;
                }
                else
                {
                    heading = headCell.CellValue.Text;
                }

                //除开表头之后的单元格数据数据
                IEnumerable<Cell> dataCells = cells.Skip(1);
                dataArray = new string[dataCells.Count()];

                for (int i = 0; i < dataCells.Count(); i++)
                {
                    Cell dataCell = dataCells.ElementAt(i);

                    if (dataCell.DataType != null && dataCell.DataType.Value == CellValues.SharedString)
                    {
                        SharedStringTablePart shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                        dataArray[i] = items[int.Parse(dataCell.CellValue.Text)].InnerText;
                    }
                    else
                    {
                        dataArray[i] = dataCell.CellValue.Text;
                    }
                }
            }
            return new Tuple<string, string[]>(heading, dataArray);
        }

        /// <summary>
        /// 根据文件地址、工作表名以及单元格名字获取列名和单元格的数据
        /// </summary>
        /// <param name="docName">文件地址</param>
        /// <param name="worksheetName">工作表名</param>
        /// <param name="cellName">单元格名</param>
        /// <returns>列名</returns>
        public static Tuple<string, string[]> GetColumnHeadingAndDataFromStream(Stream dataStream, string worksheetName, string cellName)
        {
            string heading;
            string[] dataArray;
            //打开文件
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(dataStream, false))
            {
                IEnumerable<Sheet> sheets = document.WorkbookPart.Workbook.Descendants<Sheet>().
                    Where(s => s.Name == worksheetName);
                if (sheets.Count() == 0)
                {
                    return null;
                }
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheets.First().Id);

                //获取特定单元格的列名
                string columnName = GetColumnName(cellName);

                //根据列名获取单元格的行数据（按照行序号升序排序）
                IEnumerable<Cell> cells = worksheetPart.Worksheet.Descendants<Cell>().
                    Where(c => string.Compare(GetColumnName(c.CellReference.Value), columnName, true) == 0).
                    OrderBy(r => GetRowIndex(r.CellReference));

                if (cells.Count() == 0)
                {
                    return null;
                }

                Cell headCell = cells.First();

                if (headCell.DataType != null && headCell.DataType.Value == CellValues.SharedString)
                {
                    SharedStringTablePart shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                    SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                    heading = items[int.Parse(headCell.CellValue.Text)].InnerText;
                }
                else
                {
                    heading = headCell.CellValue.Text;
                }

                //除开表头之后的单元格数据数据
                IEnumerable<Cell> dataCells = cells.Skip(1);
                dataArray = new string[dataCells.Count()];

                for (int i = 0; i < dataCells.Count(); i++)
                {
                    Cell dataCell = dataCells.ElementAt(i);

                    if (dataCell.DataType != null && dataCell.DataType.Value == CellValues.SharedString)
                    {
                        SharedStringTablePart shareStringPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

                        dataArray[i] = items[int.Parse(dataCell.CellValue.Text)].InnerText;
                    }
                    else
                    {
                        dataArray[i] = dataCell.CellValue.Text;
                    }
                }
            }
            return new Tuple<string, string[]>(heading, dataArray);
        }

    }
}
