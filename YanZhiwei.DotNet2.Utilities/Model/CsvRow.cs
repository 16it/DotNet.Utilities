using System.Collections.Generic;

namespace YanZhiwei.DotNet2.Utilities.Model
{
    /// <summary>
    /// CSV 行数据
    /// </summary>
    public sealed class CsvRow : List<string>
    {
        /// <summary>
        /// CSV行文本
        /// </summary>
        public string RowText { get; set; }
    }
}