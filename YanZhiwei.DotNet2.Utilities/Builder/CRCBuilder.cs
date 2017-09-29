using System;

namespace YanZhiwei.DotNet2.Utilities.Builder
{
    /// <summary>
    /// CRC 辅助类
    /// </summary>
    /// 时间：2016/11/23 15:43
    /// 备注：
    public static class CRCBuilder
    {
        /// <summary>
        /// CRC-16 MODBUS实现
        /// 冗余循环校验码含2个字节
        /// </summary>
        /// <param name="data">需要计算得数组</param>
        /// <returns>CRC数值</returns>
        /// 时间：2016/11/23 15:43
        /// 备注：
        public static ushort Calu16MODBUS(byte[] data)
        {
            ushort _ax = 0xFFFF;
            ushort _lsb = 0;

            for (int i = 0; i < data.Length; i++)
            {
                _ax ^= data[i];

                for (int j = 0; j < 8; j++)
                {
                    _lsb = Convert.ToUInt16(_ax & 0x0001);
                    _ax = Convert.ToUInt16(_ax >> 1);

                    if (_lsb != 0)
                        _ax ^= 0xA001;
                }
            }

            return _ax;
        }

        /// <summary>
        /// CRC 累加和计算
        /// </summary>
        /// <param name="data">需要计算得数组</param>
        /// <returns>CRC数值</returns>
        public static byte CaluSum(Byte[] data)
        {
            byte _cal = 0x00;
            uint _totol = 0;
            foreach (Byte item in data)
            {
                _totol += item;
            }

            unchecked
            {
                _cal = (byte)_totol;
            }

            return _cal;
        }
    }
}