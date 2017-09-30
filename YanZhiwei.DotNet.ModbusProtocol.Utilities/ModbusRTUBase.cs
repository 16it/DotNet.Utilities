using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YanZhiwei.DotNet.ModbusProtocol.Utilities
{
    public class ModbusRTUBase
    {
        /// <summary>
        /// (线圈状态0x)(读PLC的输出状态) 可读可写DO
        /// </summary>
        public const byte ReadCoilStatus = 0x01;
        
        /// <summary>
        /// (输入状态1x)(读PLC的输入状态) 只读DI
        /// </summary>
        public const byte ReadInputStatus = 0x02;
        
        /// <summary>
        ///(保持寄存器4x HR) (读模出状态)读整形、状态字、浮点型、字符型，与16对应
        /// </summary>
        public const byte ReadHoldingRegister = 0x03;
        
        /// <summary>
        /// (输入寄存器3x AR) (读PLC模入状态) 读整形、状态字、浮点型 只读AI
        /// </summary>
        public const byte ReadInputRegister = 0x04;
        
        /// <summary>
        /// (强制单路输出，给PLC写数据,可读可写DO
        /// </summary>
        public const byte WriteSingleCoil = 0x05;
        
        
        /// <summary>
        ///(强制单路模出，给PLC写数据,写单个整形、 状态字、浮点型、字符型,写HR4x的地址区
        /// </summary>
        public const byte WriteSingleRegister = 0x06;
        
        /// <summary>
        /// 强制多路输出，给PLC写数据,写多个位
        /// </summary>
        public const byte WriteMultipleCoil = 0x015;
        
        /// <summary>
        /// (强制多路模出，给PLC写数据,写多个整形、 状态字、浮点型、字符型
        /// </summary>
        public const byte WriteMultipleRegister = 0x16;
    }
}
