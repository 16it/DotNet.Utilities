using System.Collections.Generic;

namespace YanZhiwei.DotNet.MVC.AdminPanel.UI.Models
{
    public class SysModuleNavModel
    {
        public string attributes { get; set; }
        public List<SysModuleNavModel> children { get; set; }
        public string iconCls { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string text { get; set; }
    }
}