using System;
using System.Collections.Generic;
using System.Windows.Forms;
using YanZhiwei.DotNet2.Utilities.Builder;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.Core;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet2.Utilities.WinFromExamples
{
    public partial class FormUnPackageData : Form
    {
        public FormUnPackageData()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }
        
        /// <summary>
        /// 添加显示操作日志
        /// </summary>
        /// <param name="log">日志内容</param>
        /// 时间：2016/8/23 13:52
        /// 备注：
        private void AddOptLog(string log)
        {
            try
            {
                if(!string.IsNullOrEmpty(log))
                {
                    log = string.Format("{0} {1}", DateTime.Now.FormatDate(1), log);
                    listLog.UIThread<ListBox>(ls =>
                    {
                        ls.Items.Add(log);
                        ls.SelectedIndex = ls.Items.Count - 1;
                    });
                }
            }
            catch(System.Exception ex)
            {
                ex.Data.Add("log", log);
            }
        }
        
        private CtmUnPackageData ctuUnpackageData = new CtmUnPackageData(new CtmUnPackageProtocol());
        
        /// <summary>
        /// Handles the DataReceived event of the serialPort1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.IO.Ports.SerialDataReceivedEventArgs"/> instance containing the event data.</param>
        /// 时间:2017/1/9 22:20
        /// 备注:
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] _curBuffer = new byte[serialPort1.BytesToRead];
            
            if(_curBuffer.Length > 0)
            {
                int _curBufferLength = serialPort1.Read(_curBuffer, 0, _curBuffer.Length);
                string _hexString = ByteHelper.ToHexStringWithBlank(_curBuffer);
                AddOptLog(_hexString);
                ctuUnpackageData.DataBufferReceiving(_curBuffer);
            }
        }
        
        private void winUnPackageData_Load(object sender, EventArgs e)
        {
            ctuUnpackageData.ReceivedFullPackageEvent += CtuUnpackageData_ReceivedFullPackageEvent;
        }
        
        private void CtuUnpackageData_ReceivedFullPackageEvent(byte[] packageData)
        {
            string _hexString = ByteHelper.ToHexStringWithBlank(packageData);
            AddOptLog("接受完整报文：" + _hexString);
        }
    }
    
    public class CtmUnPackageData : UnPackageData
    {
        //86
        //2E 00
        //01 02 03 04 05 06 07 08
        //86
        //90 02 00 00 02 F1 12 70 89 57 54 46 23 12 67 33 33 33 33 33 33 33 33 33 33 33 33 33 33 33 20 17 01 09 10 02 06 05 00 01 74 00 43 01 01 00
        //30 4D
        //16
        public CtmUnPackageData(IUnPackageProtocol iUnPackage) : base(iUnPackage, 0x86, 0x16, 65535, 15)
        {
        }
    }
    
    /// <summary>
    /// 台区协议拆包接口实现
    /// </summary>
    /// 时间:2017/1/9 21:56
    /// 备注:
    /// <seealso cref="IUnPackageProtocol" />
    public class CtmUnPackageProtocol : IUnPackageProtocol
    {
        public bool CheckedCaluCrc(byte[] expect, byte[] actual)
        {
            return ArrayHelper.CompletelyEqual(expect, actual);
        }
        
        public byte[] GetCaluCrcValue(byte[] buffer)
        {
            ushort _crcValue = CRCBuilder.Calu16MODBUS(buffer);
            return ByteHelper.ToBytes(_crcValue, false);
        }
        
        public byte[] GetProtocolCaluCRCSection(byte[] buffer)
        {
            return ArrayHelper.Copy<byte>(buffer, 1, buffer.Length - 3);
        }
        
        public byte[] GetProtocolCRCSection(byte[] buffer)
        {
            byte[] _crcSection = new byte[2];
            int _packageDataLength = buffer.Length;
            _crcSection[0] = buffer[_packageDataLength - 3];
            _crcSection[1] = buffer[_packageDataLength - 2];
            return _crcSection;
        }
        
        public int GetProtocolLengthSection(List<byte> buffer)
        {
            byte[] _lenSection = new byte[2];
            _lenSection[0] = buffer[1];
            _lenSection[1] = buffer[2];
            return ByteHelper.ToUInt16(_lenSection);
        }
    }
}