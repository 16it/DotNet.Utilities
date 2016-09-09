using System.Windows.Forms;

namespace YanZhiwei.DotNet.UARTSolution
{
    public class FormServiceBase : Form
    {
        public virtual IPacketDataService CommonService
        {
            get
            {
                return ServiceContext.Current.CommonService;
            }
        }
    }
}