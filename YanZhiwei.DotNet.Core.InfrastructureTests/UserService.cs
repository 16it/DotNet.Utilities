namespace YanZhiwei.DotNet.Core.InfrastructureTests
{
    public class UserService : IUserService
    {
        public string GetUserName(string name)
        {
            return name;
        }
    }
}