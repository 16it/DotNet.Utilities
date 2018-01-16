using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YanZhiwei.DotNet.Log4Net.UtilitiesExamples
{
    public class CustomRollingFileAppender : RollingFileAppender
    {
        protected override void AdjustFileBeforeAppend()
        {

            var aa = this;
            base.AdjustFileBeforeAppend();
        }
    }
}
