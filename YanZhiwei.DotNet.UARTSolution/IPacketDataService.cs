using System;

namespace YanZhiwei.DotNet.UARTSolution
{
    public interface IPacketDataService
    {
    
    
        void DataReceived(byte[] buffer);
        
        void ResetDataReceived();
        
        void VerifyingPacketData();
    }
}