using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    public class JsonException : Exception
    {
        public string Json { get; set; }

        public JsonException(string message)
            : base(message)
        {

        }

        public JsonException(string message, string json)
            : base(message)
        {
            Json = json;
        }

        public JsonException(string message, string json, Exception inner)
            : base(message, inner)
        {
            Json = json;
        }
    }
}
