using System.Windows.Forms;

namespace YanZhiwei.DotNet.UARTSolution
{
    public class FormServiceBase : Form
    {
        public virtual IPacketDataService PacketDataService
        {
            get
            {
                return ServiceContext.Current.PacketDataService;
            }
        }
    }
}