namespace YanZhiwei.DotNet.MVC.AdminPanel.Contract.Model
{
    public class UserMenu
    {
        public string menuname { get; set; } // varchar(50), null

        public int menuid { get; set; } // int, not null

        public string icon { get; set; } // varchar(50), null

        public int userid { get; set; } // int, not null

        public string username { get; set; } // nvarchar(50), not null

        public int? menuparentid { get; set; } // int, null

        public int? menusort { get; set; } // int, null

        public string linkaddress { get; set; } // varchar(100), null
    }
}