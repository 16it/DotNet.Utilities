namespace YanZhiwei.DotNet.UARTSolution
{
    public interface IPacketDataService
    {
        void ResetDataReceived();
        
        void VerifyingPacketData();
    }
}