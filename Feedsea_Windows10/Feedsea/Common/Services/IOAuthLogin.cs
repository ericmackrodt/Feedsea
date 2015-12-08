using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Services
{
    public enum BrokerStatus
    {
        Success = 0,
        UserCancel = 1,
        ErrorHttp = 2
    }

    public class BrokerResult
    {
        public string Code { get; set; }
        public BrokerStatus Status { get; set; }
    }

    public interface IOAuthLogin
    {
        Task<BrokerResult> Login(string loginUrl, string redirectUrl);
    }
}
