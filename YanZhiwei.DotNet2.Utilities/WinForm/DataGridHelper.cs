namespace YanZhiwei.DotNet2.Utilities.WinForm
{
    using Common;
    using Core;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// DataGrid 帮助类
    /// </summary>
    public static class DataGridHelper
    {
        #region Methods

        /// <summary>
        ///  将DateTimePicker应用到列编辑时候
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <param name="datePicker">DateTimePicker</param>
        /// <param name="columnIndex">应用编辑列索引</param>
        public static void ApplyDateTimePicker(this DataGridView dataGrid, DateTimePicker datePicker, int columnIndex)
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
        /// 清除绑定
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        public static void ClearDynamicBind(this DataGridView dataGrid)
        {
            BindingSource _bindingSource = new BindingSource();
            _bindingSource.DataSource = null;
            dataGrid.DataSource = _bindingSource;
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <returns>行数</returns>
        public static int GetDynamicBindRowCount(this DataGridView dataGrid)
        {
            if (dataGrid.DataSource is BindingSource)
            {
                BindingSource _source = (BindingSource)dataGrid.DataSource;
                return _source.Count;
            }
            return 0;
        }

        /// <summary>
        /// 添加checkbox 列头
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="headerText">列名称</param>
        public static void ApplyHeaderCheckbox(this DataGridView dataGrid, int columnIndex, string headerText)
        {
            DatagridViewCheckBoxHeaderCell _checkedBox = new DatagridViewCheckBoxHeaderCell();
            dataGrid.Columns[columnIndex].HeaderCell = _checkedBox;
            dataGrid.Columns[columnIndex].HeaderText = headerText;
            _checkedBox.OnCheckBoxClicked += (state) =>
            {
                int _count = dataGrid.Rows.Count;

                for (int i = 0; i < _count; i++)
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGrid.Rows[i].Cells[columnIndex];
                    checkCell.Value = state;
                }
            };
        }

        /// <summary>
        ///根据cell内容调整其宽度
        /// </summary>
        /// <param name="girdview">DataGridView</param>
        public static void AutoCellWidth(this DataGridView girdview)
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
        public static void DrawSequenceNumber(this DataGridView dataGrid)
        {
            dataGrid.RowPostPaint += (sender, e) =>
            {
                DataGridView _dataGrid = sender as DataGridView;
                Rectangle _rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, _dataGrid.RowHeadersWidth, e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                      _dataGrid.RowHeadersDefaultCellStyle.Font,
                                      _rectangle,
                                      _dataGrid.RowHeadersDefaultCellStyle.ForeColor,
                                      TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            };
        }

        /// <summary>
        /// DataGridView绑定
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <param name="source">数据源</param>
        public static void DynamicBind<T>(this DataGridView dataGrid, IList<T> source)
        where T : class
        {
            BindingSource _bindingSource = null;

            if (dataGrid.DataSource is BindingSource)
            {
                _bindingSource = (BindingSource)dataGrid.DataSource;
                _bindingSource.AllowNew = true;

                foreach (T entity in source)
                {
                    _bindingSource.Add(entity);
                }
            }
            else
            {
                BindingList<T> _bindinglist = new BindingList<T>(source);
                _bindingSource = new BindingSource(_bindinglist, null);
                dataGrid.DataSource = _bindingSource;
            }
        }

        /// <summary>
        /// DataGridView绑定
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <param name="item">数据源</param>
        public static void DynamicBind<T>(this DataGridView dataGrid, T item)
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
        /// 获取选中行
        /// </summary>
        /// <param name="dataGrid">DataGridView对象</param>
        /// <returns>若未有选中行则返回NULL</returns>
        /// 时间：2015-12-09 17:07
        /// 备注：
        public static DataGridViewRow SelectedRow(this DataGridView dataGrid)
        {
            DataGridViewSelectedRowCollection _selectedRows = dataGrid.SelectedRows;

            if (_selectedRows != null && _selectedRows.Count > 0)
            {
                return _selectedRows[0];
            }

            return null;
        }

        /// <summary>
        /// 选中最后一行
        /// </summary>
        /// <param name="dataGrid">DataGridView</param>
        public static void SelectedLastRow(this DataGridView dataGrid)
        {
            int _lastRowIndex = dataGrid.Rows.Count - 1;
            dataGrid.ClearSelection();
            dataGrid.Rows[_lastRowIndex].Selected = true;
            dataGrid.FirstDisplayedScrollingRowIndex = _lastRowIndex;
        }

        #endregion Methods
    }
}