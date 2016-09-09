using YanZhiwei.DotNet.Core.Service;

namespace YanZhiwei.DotNet.UARTSolution.Core
{
    public class ProjectSerivce
    {
        private static ServiceHelper refSerice = new ServiceHelper();
        
        public static ServiceHelper RefService
        {
            get
            {
                return refSerice;
            }
        }
    }
}