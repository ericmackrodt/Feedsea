using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Api
{
    public class HtmlResponseException : Exception
    {
        public HtmlResponseException(string message)
            : base(message) { }
    }
}
