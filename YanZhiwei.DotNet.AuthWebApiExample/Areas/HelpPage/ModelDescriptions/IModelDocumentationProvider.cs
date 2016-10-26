using System;
using System.Reflection;

namespace YanZhiwei.DotNet.AuthWebApiExample.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}