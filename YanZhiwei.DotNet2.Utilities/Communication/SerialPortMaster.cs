namespace YanZhiwei.DotNet2.Utilities.Communication
{
    using Common;
    using Operator;
    using System;
    using System.IO.Ports;

    /// <summary>
    ///串口帮助类
    /// </summary>
    public static class SerialPortMaster
    {
        #region Fields

        private static string[] baudRate;
        private static string[] dataBits;
        private static string[] parity;
        private static string[] stopBits;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 波特率
        /// </summary>
        public static string[] BaudRates
        {
            get
            {
                if(baudRate == null)
                    baudRate = new string[] { "600", "1200", "1800", "2400", "4800", "7200", "9600", "14400", "19200", "38400", "57600", "115200", "128000" };

                return baudRate;
            }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public static string[] DataBits
        {
            get
            {
                if(dataBits == null)
                    dataBits = new string[] { "4", "5", "6", "7", "8" };

                return dataBits;
            }
        }

        /// <summary>
        /// 效验_英文
        /// </summary>
        public static string[] Paritys
        {
            get
            {
                return Enum.GetNames(typeof(Parity));
            }
        }

        /// <summary>
        /// 效验_中文
        /// </summary>
        public static string[] Paritys_CH
        {
            get
            {
                if(parity == null)
                {
                    string[] _paritys = Enum.GetNames(typeof(Parity));
                    parity = new string[_paritys.Length];
                    int i = 0;

                    foreach(string pt in _paritys)
                    {
                        if(string.Compare(pt, "None", true) == 0)
                            parity[i] = "无";
                        else if(string.Compare(pt, "Odd", true) == 0)
                            parity[i] = "奇";
                        else if(string.Compare(pt, "Even", true) == 0)
                            parity[i] = "偶";
                        else if(string.Compare(pt, "Mark", true) == 0)
                            parity[i] = "标志";
                        else if(string.Compare(pt, "Space", true) == 0)
                            parity[i] = "空格";
                        else
                            parity[i] = pt;

                        i++;
                    }
                }

                return parity;
            }
        }

        /// <summary>
        /// 串口列表
        /// </summary>
        public static string[] PortNames
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public static string[] StopBits
        {
            get
            {
                return Enum.GetNames(typeof(StopBits));
            }
        }

        /// <summary>
        /// 停止位_英文转移成数字
        /// </summary>
        public static string[] StopBits_CH
        {
            get
            {
                if(stopBits == null)
                {
                    string[] _stopBitses = Enum.GetNames(typeof(StopBits));
                    int i = 0;
                    stopBits = new string[_stopBitses.Length - 1];

                    foreach(string item in _stopBitses)
                    {
                        if(string.Compare(item, "None", true) == 0)
                            continue;
                        else if(string.Compare(item, "One", true) == 0)
                            stopBits[i] = "1";
                        else if(string.Compare(item, "Two", true) == 0)
                            stopBits[i] = "2";
                        else if(string.Compare(item, "OnePointFive", true) == 0)
                            stopBits[i] = "1.5";
                        else
                            stopBits[i] = item;

                        i++;
                    }
                }

                return stopBits;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="serialPort">串口组件</param>
        /// <param name="portName">需要关闭的串口名称</param>
        public static void Close(this SerialPort serialPort, string portName)
        {
            if(serialPort.IsOpen)
                serialPort.Close();
        }

        /// <summary>
        /// 串口发送数据
        /// </summary>
        /// <param name="serialPort">SerialPort</param>
        /// <param name="data">二进制数据</param>
        /// 时间：2016/8/24 15:37
        /// 备注：
        public static void SendData(this SerialPort serialPort, byte[] data)
        {
            ValidateOperator.Begin().NotNull(serialPort, "串口").Check<ArgumentException>(() => serialPort.IsOpen, "串口尚未打开！").NotNull(data, "串口发送数据");
            serialPort.Write(data, 0, data.Length);
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="serialPort">串口</param>
        /// <param name="portName">需要打开串口的名称</param>
        /// <param name="dateBits">数据位</param>
        /// <param name="bondRate">波特率</param>
        /// <param name="parity">效验</param>
        /// <param name="stopBit">停止位</param>
        public static void Open(this SerialPort serialPort, string portName, string dateBits, string bondRate, string parity, string stopBit)
        {
            ValidateOperator.Begin().IsInt(bondRate, "波特率").IsInt(dateBits, "数据位");

            if(serialPort.IsOpen)
                serialPort.Close();

            serialPort.PortName = portName;
            serialPort.BaudRate = bondRate.ToInt32OrDefault(9600);
            serialPort.DataBits = dateBits.ToInt32OrDefault(8);

            switch(stopBit)
            {
                case "1":
                    serialPort.StopBits = System.IO.Ports.StopBits.One;
                    break;

                case "1.5":
                    serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;

                case "2":
                    serialPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;

                default:
                    serialPort.StopBits = System.IO.Ports.StopBits.None;
                    break;
            }

            switch(parity)
            {
                case "偶":
                    serialPort.Parity = Parity.Even;
                    break;

                case "奇":
                    serialPort.Parity = Parity.Odd;
                    break;

                case "空格":
                    serialPort.Parity = Parity.Space;
                    break;

                case "标志":
                    serialPort.Parity = Parity.Mark;
                    break;

                case "无":
                    serialPort.Parity = Parity.None;
                    break;

                default:
                    serialPort.Parity = Parity.None;
                    break;
            }

            serialPort.Open();
        }

        #endregion Methods
    }
}