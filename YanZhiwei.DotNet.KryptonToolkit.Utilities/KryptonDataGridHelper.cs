using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.KryptonToolkit.Utilities
{
    /// <summary>
    /// KryptonDataGrid控件辅助类
    /// </summary>
    public static class KryptonDataGridHelper
    {
        #region Methods

        /// <summary>
        /// 清除绑定
        /// </summary>
        /// <param name="dataGrid">KryptonDataGridView</param>
        public static void ClearDynamicBind(this KryptonDataGridView dataGrid)
        {
            BindingSource _bindingSource = new BindingSource();
            _bindingSource.DataSource = null;
            dataGrid.DataSource = _bindingSource;
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="dataGrid">KryptonDataGridView</param>
        /// <returns>行数</returns>
        public static int GetDynamicBindRowCount(this KryptonDataGridView dataGrid)
        {
            if (dataGrid.DataSource is BindingSource)
            {
                BindingSource _source = (BindingSource)dataGrid.DataSource;
                return _source.Count;
            }
            return 0;
        }

        /// <summary>
        ///  将DateTimePicker应用到列编辑时候
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <param name="datePicker">DateTimePicker</param>
        /// <param name="columnIndex">应用编辑列索引</param>
        public static void ApplyDateTimePicker(this KryptonDataGridView dataGrid, DateTimePicker datePicker, int columnIndex)
        {
            datePicker.Visible = false;
            datePicker.ValueChanged += (sender, e) =>
            {
                DateTimePicker _datePicker = sender as DateTimePicker;
                dataGrid.CurrentCell.Value = _datePicker.Value;
                datePicker.Visible = false;
            };
            dataGrid.CellClick += (sender, e) =>
            {
                if (e.ColumnIndex == columnIndex)
                {
                    DataGridView _dataGrid = sender as DataGridView;
                    Rectangle _cellRectangle = _dataGrid.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    datePicker.Location = _cellRectangle.Location;
                    datePicker.Width = _cellRectangle.Width;

                    try
                    {
                        datePicker.Value = _dataGrid.CurrentCell.Value.ToDateOrDefault(DateTime.Now);
                    }
                    catch
                    {
                        datePicker.Value = DateTime.Now;
                    }

                    datePicker.Visible = true;
                }
            };
            dataGrid.Controls.Add(datePicker);
        }

        /// <summary>
        ///根据cell内容调整其宽度
        /// </summary>
        /// <param name="girdview">KryptonDataGridView</param>
        public static void AutoCellWidth(this KryptonDataGridView girdview)
        {
            int _columnSumWidth = 0;

            for (int i = 0; i < girdview.Columns.Count; i++)
            {
                girdview.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                _columnSumWidth += girdview.Columns[i].Width;
            }

            girdview.AutoSizeColumnsMode = _columnSumWidth > girdview.Size.Width ? DataGridViewAutoSizeColumnsMode.DisplayedCells : DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        ///  绘制行号
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        public static void DrawSequenceNumber(this KryptonDataGridView dataGrid)
        {
            dataGrid.RowPostPaint += (sender, e) =>
            {
                KryptonDataGridView _dataGrid = sender as KryptonDataGridView;
                Rectangle _rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, _dataGrid.RowHeadersWidth, e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                      _dataGrid.RowHeadersDefaultCellStyle.Font,
                                      _rectangle,
                                      _dataGrid.RowHeadersDefaultCellStyle.ForeColor,
                                      TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            };
        }

        /// <summary>
        /// KryptonDataGridView绑定
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <param name="source">数据源</param>
        public static void DynamicBind<T>(this KryptonDataGridView dataGrid, IList<T> source)
            where T : class
        {
            BindingSource _source = null;

            if (dataGrid.DataSource is BindingSource)
            {
                _source = (BindingSource)dataGrid.DataSource;
                _source.AllowNew = true;

                foreach (T entity in source)
                {
                    _source.Add(entity);
                }
            }
            else
            {
                BindingList<T> _bindinglist = new BindingList<T>(source);
                _source = new BindingSource(_bindinglist, null);
                dataGrid.DataSource = _source;
            }
        }

        /// <summary>
        /// KryptonDataGridView绑定
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dataGrid">KryptonDataGridView</param>
        /// <param name="item">数据对象</param>
        public static void DynamicBind<T>(this KryptonDataGridView dataGrid, T item)
            where T : class
        {
            BindingSource _bindingSource = null;

            if (dataGrid.DataSource is BindingSource)
            {
                _bindingSource = (BindingSource)dataGrid.DataSource;
                _bindingSource.AllowNew = true;

                _bindingSource.Add(item);
            }
            else
            {
                List<T> _dataSource = new List<T>(1) { item };
                BindingList<T> _bindinglist = new BindingList<T>(_dataSource);
                _bindingSource = new BindingSource(_bindinglist, null);
                dataGrid.DataSource = _bindingSource;
            }
        }

        /// <summary>
        /// 选中最后一行
        /// </summary>
        /// <param name="dataGrid">KryptonDataGridView</param>
        public static void SelectedLastRow(this KryptonDataGridView dataGrid)
        {
            int _lastRowIndex = dataGrid.Rows.Count - 1;
            dataGrid.ClearSelection();
            dataGrid.Rows[_lastRowIndex].Selected = true;
            dataGrid.FirstDisplayedScrollingRowIndex = _lastRowIndex;
        }

        /// <summary>
        /// 获取选中行
        /// </summary>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <returns>若未有选中行则返回NULL</returns>
        /// 时间：2015-12-09 17:07
        /// 备注：
        public static DataGridViewRow SelectedRow(this KryptonDataGridView dataGrid)
        {
            DataGridViewSelectedRowCollection _selectedRows = dataGrid.SelectedRows;

            if (_selectedRows != null && _selectedRows.Count > 0)
            {
                return _selectedRows[0];
            }

            return null;
        }

        #endregion Methods
    }
}