using Microsoft.VisualStudio.TestTools.UnitTesting;
using YanZhiwei.DotNet2.Utilities.Builder;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet2.Utilities.Core.Tests
{
    public class CTMPackageData : PackageData
    {
        public CTMPackageData(byte[] cmdWordArray, byte[] cmdParmArray, byte[] terminalId)
        {
            CmdWordArray = cmdWordArray;
            CmdParmArray = cmdParmArray;
            TerminalId = terminalId;
        }

        /// <summary>
        /// 同步字2
        /// </summary>
        public override byte SyncWord2
        {
            get
            {
                return 0x16;
            }
        }

        /// <summary>
        /// 同步字1
        /// </summary>
        public override byte SynWord1
        {
            get
            {
                return 0x68;
            }
        }

        /// <summary>
        /// 终端ID 8个字节
        /// </summary>
        public byte[] TerminalId
        {
            get;
            set;
        }

        /// <summary>
        /// CRC校验算法
        /// </summary>
        /// <param name="data">数据包</param>
        /// <returns>CRC数值</returns>
        /// 时间：2016/8/23 15:30
        /// 备注：
        public static ushort GetCrc16(byte[] data)
        {
            ushort _crc = 0;

            for(int i = 0; i < data.Length; i++)
            {
                _crc += data[i];
            }

            return _crc;
        }

        /// <summary>
        /// 通过构造函数初始化参数，来将对象转换为BYTE数组
        /// </summary>
        /// <returns>
        /// BYTE数组
        /// </returns>
        /// 时间：2016/8/30 17:21
        /// 备注：
        public override byte[] ToBytes()
        {
            byte[] _cmdLengthDataPart = null;
            using(ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append(CmdWordArray);

                if(CmdParmArray != null)
                    builder.Append(CmdParmArray);

                _cmdLengthDataPart = builder.ToArray();
            }
            byte[] _packetLength = ByteHelper.ToBytes((ushort)_cmdLengthDataPart.Length, false).Resize<byte>(2);//Length占位
            byte[] _cmdCRCDataPart = null;
            using(ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append(_packetLength);//长度
                builder.Append(TerminalId);//CTM 终端ID
                builder.Append(SynWord1);
                builder.Append(CmdWordArray);

                if(CmdParmArray != null)
                    builder.Append(CmdParmArray);//用户数据

                _cmdCRCDataPart = builder.ToArray();
            }
            byte[] _cmdAll = null;
            using(ByteArrayBuilder builder = new ByteArrayBuilder())
            {
                builder.Append(SynWord1);
                builder.Append(_cmdCRCDataPart);
                builder.Append(ByteHelper.ToBytes(GetCrc16(_cmdCRCDataPart), false)[0]);
                builder.Append(SyncWord2);
                _cmdAll = builder.ToArray();
            }
            return _cmdAll;
        }
    }

    [TestClass()]
    public class PackageDataTests
    {
        [TestMethod()]
        public void ToBytesTest()
        {
            byte[] _cmdWordArray = new byte[2] { 0xA8, 0x06 };
            byte[] _cmdParmArray = new byte[3] { 0x01, 0x00, 0x00 };
            byte[] _cmdTerminalIdArray = new byte[8] { 0xFC, 0x55, 0x5F, 0xFF, 0x0D, 0xB0, 0x2D, 0x00 };
            PackageData _packageData = new CTMPackageData(_cmdWordArray, _cmdParmArray, _cmdTerminalIdArray);
            byte[] _expect = new byte[19] { 0x68, 0x05, 0x00, 0xFC, 0x55, 0x5F, 0xFF, 0x0D, 0xB0, 0x2D, 0x00, 0x68, 0xA8, 0x06, 0x01, 0x00, 0x00, 0xB5, 0x16 };
            byte[] _actual = _packageData.ToBytes();
            CollectionAssert.AreEqual(_actual, _expect);
        }
    }
}