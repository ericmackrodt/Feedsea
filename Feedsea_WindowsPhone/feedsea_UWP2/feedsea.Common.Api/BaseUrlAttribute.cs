using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    [AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
    public class BaseUrlAttribute : Attribute
    {
        public string BaseUrl { get; set; }

        public BaseUrlAttribute(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public static string GetAttributeValue(Type type)
        {
            var attrib = (BaseUrlAttribute)type.GetTypeInfo().GetCustomAttribute(typeof(BaseUrlAttribute));
            return attrib == null ? null : attrib.BaseUrl;
        }
    }
}
