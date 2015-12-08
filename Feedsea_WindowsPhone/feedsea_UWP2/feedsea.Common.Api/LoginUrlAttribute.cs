using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class LoginUrlAttribute : Attribute
    {
        public string BaseUrl { get; set; }

        public LoginUrlAttribute(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public static string GetAttributeValue(Type type)
        {
            var attrib = (LoginUrlAttribute)type.GetTypeInfo().GetCustomAttribute(typeof(LoginUrlAttribute));
            return attrib == null ? null : attrib.BaseUrl;
        }
    }
}
