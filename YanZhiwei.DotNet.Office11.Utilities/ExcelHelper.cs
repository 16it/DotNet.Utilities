namespace YanZhiwei.DotNet.Office11.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using Microsoft.Office.Interop.Excel;

    using YanZhiwei.DotNet2.Utilities.Common;
    using YanZhiwei.DotNet2.Utilities.Operator;

    /// <summary>
    /// Excel 帮助类
    /// </summary>
    public class ExcelHelper
    {
        #region Methods

        /// <summary>
        ///  将数据集合导出到excel
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">导出到EXCEL的数据集合</param>
        /// <param name="sheetName">SheetName</param>
        /// <param name="excelPath">导出EXCEL路径</param>
        /// 时间：2016/9/1 11:53
        /// 备注：
        public static void ToExcel<T>(IEnumerable<T> source, string sheetName, string excelPath)
            where T : class
        {
            ValidateOperator.Begin().NotNull(source, "导出到EXCEL的数据集合").NotNullOrEmpty(sheetName, "sheetName");
            IDictionary<string, IEnumerable<T>> _source = new Dictionary<string, IEnumerable<T>>();
            _source.Add(sheetName, source);
            ToExcel<T>(_source, excelPath);
        }

        /// <summary>
        /// 将集合字典导出到excel
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="sourceDic">数据集合字典</param>
        /// <param name="excelpath">保存路径</param>
        public static void ToExcel<T>(IDictionary<string, IEnumerable<T>> sourceDic, string excelpath)
            where T : class
        {
            ValidateOperator.Begin().NotNull(sourceDic, "导出到EXCEL的数据集合字典").NotNullOrEmpty(excelpath, "EXCEL路径").IsFilePath(excelpath, "EXCEL路径");

            object _missingValue = Missing.Value;
            Application _application = null;
            Workbooks _books = null;
            _Workbook _book = null;
            Sheets _sheets = null;
            _Worksheet _workSheet = null;
            Range _range = null;
            Microsoft.Office.Interop.Excel.Font _font = null;
            object _optionalValue = Missing.Value;
            string _headerStart = "A1", _dataStart = "A2";

            _application = new Application();
            _books = (Workbooks)_application.Workbooks;
            _book = (_Workbook)(_books.Add(_optionalValue));
            _sheets = (Sheets)_book.Worksheets;

            int _sheetCount = sourceDic.Count;

            foreach(KeyValuePair<string, IEnumerable<T>> item in sourceDic)
            {
                _workSheet = (_Worksheet)(_sheets.Add(Type.Missing, Type.Missing, 1, Type.Missing));
                _workSheet.Name = item.Key;
                IDictionary<string, string> _headers = ReflectHelper.GetPropertyName<T>();
                int _headerColCount = _headers.Count;
                _range = _workSheet.get_Range(_headerStart, _optionalValue);
                _range = _range.get_Resize(1, _headerColCount);
                string[] _headerCol = new string[_headerColCount];
                _headers.Values.CopyTo(_headerCol, 0);
                _range.set_Value(_optionalValue, _headerCol);
                _range.BorderAround(Type.Missing, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                _font = _range.Font;
                _font.Bold = true;
                _range.Interior.Color = Color.LightGray.ToArgb();
                int _rowCount = item.Value.Count();
                object[,] _dataArray = new object[_rowCount, _headers.Count];
                int j = 0;

                foreach(T itemObj in item.Value)
                {
                    int i = 0;

                    foreach(KeyValuePair<string, string> entry in _headers)
                    {
                        object _value = typeof(T).InvokeMember(entry.Key, BindingFlags.GetProperty, null, itemObj, null);
                        _dataArray[j, i++] = (_value == null) ? "" : _value.ToString();
                    }

                    j++;
                }

                _range = _workSheet.get_Range(_dataStart, _optionalValue);
                _range = _range.get_Resize(_rowCount, _headers.Count);
                _range.set_Value(_optionalValue, _dataArray);
                _range.BorderAround(Type.Missing, XlBorderWeight.xlThin, XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                _range = _workSheet.get_Range(_headerStart, _optionalValue);
                _range = _range.get_Resize(_rowCount + 1, _headers.Count);
                _range.Columns.AutoFit();
            }

            int _workSheetCount = _book.Worksheets.Count;

            if(_workSheetCount > _sheetCount)
            {
                while(_workSheetCount > _sheetCount)
                {
                    ((Worksheet)_book.Worksheets[_workSheetCount]).Delete();
                    _workSheetCount--;
                }
            }

            _book.SaveAs(excelpath, _missingValue, _missingValue, _missingValue, false, false, XlSaveAsAccessMode.xlNoChange, _missingValue, _missingValue, _missingValue, _missingValue, _missingValue);
            _application.Quit();

            try
            {
                if(_workSheet != null)
                    Marshal.ReleaseComObject(_workSheet);

                _workSheet = null;

                if(_sheets != null)
                    Marshal.ReleaseComObject(_sheets);

                _sheets = null;

                if(_book != null)
                    Marshal.ReleaseComObject(_book);

                _book = null;

                if(_books != null)
                    Marshal.ReleaseComObject(_books);

                _books = null;

                if(_application != null)
                    Marshal.ReleaseComObject(_application);

                _application = null;
            }
            catch
            {
                _workSheet = null;
                _sheets = null;
                _book = null;
                _books = null;
                _application = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        private static void ReleaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch(Exception)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        #endregion Methods
    }
}