using System;
using System.Collections.Generic;
using YanZhiwei.DotNet2.Utilities.Common;

namespace YanZhiwei.DotNet.UARTSolution
{
    public class CTMUnPacketDataService : IPacketDataService
    {
        public Action<byte[]> HanlderPacketDataReceived;
        private const byte startFlag = 0x68;
        private static readonly object syncObject = new object();
        
        //指令开始Flag
        private static List<byte> cacheBuffer = null;
        
        private static bool receiving = false;//判断是否在接受协议数据中
        //缓冲Buffer
        
        public void DataReceived(byte[] buffer)
        {
            lock(syncObject)
            {
                if(buffer[0] == startFlag && !receiving)
                {
                    receiving = true;
                    cacheBuffer = new List<byte>();
                }
                
                if(receiving)
                {
                    cacheBuffer.AddRange(buffer);
                    VerifyingPacketData();
                }
                else
                {
                    ResetDataReceived();
                }
            }
        }
        
        public void ResetDataReceived()
        {
            receiving = false;
            cacheBuffer = null;
        }
        
        public void VerifyingPacketData()
        {
            if(cacheBuffer.Count >= 16)
            {
                bool _checkedCrc = false;
                byte[] _packetDataBuffer = VerifyingPacketDataLength(out _checkedCrc);
                
                if(VerifyingPacketCRC(_checkedCrc, _packetDataBuffer))
                {
                    HanlderPacketDataReceived(_packetDataBuffer);
                }
            }
            else if(cacheBuffer.Count >= 65535)
            {
                ResetDataReceived();
            }
        }
        
        /// <summary>
        /// 数据包CRC校验
        /// 从区域B到区域D的校验和，低位在前，高位在后
        /// </summary>
        /// 时间：2016/8/23 15:01
        /// 备注：
        private bool VerifyingPacketCRC(bool checkCRC, byte[] packetDataBuffer)
        {
            bool _result = false;
            
            if(checkCRC && packetDataBuffer != null)
            {
                byte[] _packetDataCRC = ArrayHelper.Copy<byte>(cacheBuffer.ToArray(), 1, packetDataBuffer.Length - 2);
                byte _expectCRC = ByteHelper.ToBytes(GetCrc16(_packetDataCRC), false)[0];
                byte _actualCRC = cacheBuffer[cacheBuffer.Count - 2];
                
                if(_expectCRC == _actualCRC)
                {
                    _result = true;
                }
                
                ResetDataReceived();
            }
            
            return _result;
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
        /// 检验数据包长度是否合法
        /// 命令字+用户数据的长度+效验码
        /// </summary>
        /// <returns>是否合法</returns>
        /// 时间：2016/8/23 14:55
        /// 备注：
        private byte[] VerifyingPacketDataLength(out bool checkedCRC)
        {
            checkedCRC = false;
            byte[] _packetDataBuffer = null;
            byte[] _dataPackageLengthArray = new byte[2] { cacheBuffer[2], cacheBuffer[1] };
            int _dataPackageLength = 0;// ByteHelper.ToHexString(_dataPackageLengthArray, 0, 2);
            
            if((_dataPackageLength + 14) <= cacheBuffer.Count)
            {
                _packetDataBuffer = ArrayHelper.Copy<byte>(cacheBuffer.ToArray(), 0, _dataPackageLength + 14);
                checkedCRC = true;
            }
            
            return _packetDataBuffer;
        }
    }
}