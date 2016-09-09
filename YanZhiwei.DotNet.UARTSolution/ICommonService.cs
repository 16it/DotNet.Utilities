using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YanZhiwei.DotNet.UARTSolution
{
    public interface ICommonService
    {
        void WriteLog();
        void SendData(string hexString);
    }
}
