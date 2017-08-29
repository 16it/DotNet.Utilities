using System.Collections.Generic;
using YanZhiwei.DotNet.Core.ServiceTests.Model;

namespace YanZhiwei.DotNet.Core.ServiceTests.Events
{
    public interface IUserService
    {
        string GetUserName(string Name);

        string InsertUser(string Name);

        string UpdateUser(string Name);

        string DeleteUser(string Name);
    }
}