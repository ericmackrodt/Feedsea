using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Helpers
{
    public static class ConnectionVerifier
    {
        public static TOut Verify<TIn, TOut>(this Func<TIn, TOut> call, TIn arg, Action onError = null)
        {
            try
            {
                return call(arg);
            }
            finally
            {
                
            }
        }
    }
}
