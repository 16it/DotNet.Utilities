using ComponentFactory.Krypton.Toolkit;
using System;
using System.IO.Ports;
using System.Windows.Forms;
using YanZhiwei.DotNet.UARTSolution;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Communication;
using YanZhiwei.DotNet2.Utilities.Operator;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet.UARTSolutionUI
{
    /// <summary>
    /// 主界面
    /// </summary>
    /// 时间：2016/8/23 13:19
    /// 备注：
    public partial class FormMain : FormServiceBase
    {
        public FormMain()
        {
            InitializeComponent();
        }
        
        private void FormMain_Load(object sender, System.EventArgs e)
        {
            HanlderFormClosed();
            InitUIBasic();
            InitSerialPortUIBasic();
        }
        
        /// <summary>
        /// 处理程序关闭时候提醒
        /// </summary>
        private void HanlderFormClosed()
        {
            ApplicationHelper.CapturedExit<FormMain>(this, () =>
            {
                if(MessageHelper.ShowYesNoAndTips("确认退出软件？") == DialogResult.Yes)
                    return true;
                    
                return false;
            });
        }
        
        /// <summary>
        /// 绑定串口UI数据
        /// </summary>
        private void InitSerialPortUIBasic()
        {
            cbSeriportDataBits.DataSource = SerialPortMaster.DataBits;
            cbSeriportBaudRates.DataSource = SerialPortMaster.BaudRates;
            cbSeriportName.DataSource = SerialPortMaster.PortNames;
            cbSeriportParitys.DataSource = SerialPortMaster.Paritys_CH;
            cbSeriportStopBits.DataSource = SerialPortMaster.StopBits_CH;
            cbSeriportDataBits.Text = "8";
            cbSeriportBaudRates.Text = "9600";
        }
        
        /// <summary>
        /// 设定串口UI是否可用
        /// </summary>
        /// <param name="enable">是否可用</param>
        private void SetSerialPortUIEnable(bool enable)
        {
            cbSeriportDataBits.Enabled = enable;
            cbSeriportBaudRates.Enabled = enable;
            cbSeriportName.Enabled = enable;
            cbSeriportParitys.Enabled = enable;
            cbSeriportStopBits.Enabled = enable;
            btnSerilportOpt.Text = enable == true ? "打开串口" : "关闭串口";
            btnSerilportOpt.Tag = enable == true ? "1" : "0";
        }
        
        /// <summary>
        /// UI基本初始化
        /// </summary>
        private void InitUIBasic()
        {
            treeFunctionList.ExpandAll();
        }
        
        private void treeFunctionList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            short _selectIndex = Convert.ToInt16(e.Node.Name);
            tabFunctionList.SelectedIndex = _selectIndex;
        }
        
        /// <summary>
        /// 打开或者关闭串口
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnSerilportOpt_Click(object sender, EventArgs e)
        {
            string _tag = btnSerilportOpt.Tag.ToString();
            string _serilportName = cbSeriportName.Text.Trim();
            
            try
            {
                ValidateOperator.Begin().NotNullOrEmpty(_serilportName, "串口名称");
                PacketDataService.ResetDataReceived();
                
                if(_tag == "1")
                {
                    SerialPortMaster.Open(serialPort, _serilportName, cbSeriportDataBits.Text, cbSeriportBaudRates.Text, cbSeriportParitys.Text, cbSeriportStopBits.Text);
                    SetSerialPortUIEnable(false);
                    AddOptLog(string.Format("打开串口[{0}]成功。", _serilportName));
                }
                else
                {
                    SerialPortMaster.Close(serialPort, _serilportName);
                    SetSerialPortUIEnable(true);
                    AddOptLog(string.Format("关闭串口[{0}]成功。", _serilportName));
                }
            }
            catch(ArgumentNullException ex)
            {
                MessageHelper.ShowWarning(ex.ParamName);
            }
            catch(Exception ex)
            {
                string _errorMsg = string.Format("{0}[{1}]串口失败！", _tag == "0" ? "打开" : "关闭", _serilportName);
                MessageHelper.ShowError(_errorMsg);
                ServiceContext.Current.LogException(ex, _errorMsg);
            }
        }
        
        /// <summary>
        /// 添加显示操作日志
        /// </summary>
        /// <param name="log">日志内容</param>
        private void AddOptLog(string log)
        {
            try
            {
                if(!string.IsNullOrEmpty(log))
                {
                    log = string.Format("{0} {1}", DateTime.Now.FormatDate(1), log);
                    listLog.UIThread<KryptonListBox>(ls =>
                    {
                        ls.Items.Add(log);
                        ls.SelectedIndex = ls.Items.Count - 1;
                    });
                }
            }
            catch(Exception ex)
            {
                ex.Data.Add("log", log);
                ServiceContext.Current.LogException(ex, "添加日志失败");
            }
        }
        
        /// <summary>
        /// 串口数据接受事件
        /// </summary>
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] _curBuffer = new byte[serialPort.BytesToRead];
            
            if(_curBuffer.Length > 0)
            {
                int _curBufferLength = serialPort.Read(_curBuffer, 0, _curBuffer.Length);
                PacketDataService.DataReceived(_curBuffer);
                string _curBufferString = ByteHelper.ToHexStringWithBlank(ArrayHelper.Copy<byte>(_curBuffer, 0, _curBufferLength));
                AddOptLog(string.Format("接收:{0}.", _curBufferString));
            }
        }
    }
}