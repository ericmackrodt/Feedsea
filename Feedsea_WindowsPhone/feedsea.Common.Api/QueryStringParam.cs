using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    public class QueryStringParam
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public QueryStringParam(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
