using System.ComponentModel;

namespace YanZhiwei.DotNet3._5.Utilities.WPF
{
    /// <summary>
    /// 绑定数据属性。这个类的作用是实现了INotifyPropertyChanged接口。WPF中类要实现这个接口，其属性成员才具备通知UI的能力，数据绑定。
    /// </summary>
    /// 时间：2016/7/4 16:41
    /// 备注：
    public class NotificationObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性数值更新事件
        /// </summary>
        /// 时间：2016/7/4 16:41
        /// 备注：
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 触发属性数值更新事件
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// 时间：2016/7/4 16:41
        /// 备注：
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}