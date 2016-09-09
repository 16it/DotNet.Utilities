using System.Windows.Forms;

namespace YanZhiwei.DotNet.UARTSolution
{
    public class FormServiceBase : Form
    {
        public virtual ICommonService CommonService
        {
            get
            {
                return ServiceContext.Current.CommonService;
            }
        }
    }
}