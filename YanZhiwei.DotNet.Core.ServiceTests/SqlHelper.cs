namespace YanZhiwei.DotNet.Core.ServiceTests
{
    public interface ISqlHelper
    {
        string HelloWorld();
    }
    
    public class SqlHelper : ISqlHelper
    {
        public string HelloWorld()
        {
            return "Hello World.";
        }
    }
}