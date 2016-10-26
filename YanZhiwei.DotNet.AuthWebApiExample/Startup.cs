using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(YanZhiwei.DotNet.AuthWebApiExample.Startup))]

namespace YanZhiwei.DotNet.AuthWebApiExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
