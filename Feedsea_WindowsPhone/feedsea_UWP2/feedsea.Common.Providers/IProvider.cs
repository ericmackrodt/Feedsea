using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Providers
{
    public interface IProvider
    {
        string ServiceName { get; }
        IOauthLoginData LoginData { get; }
        Task<LoginStatus> Login();
        Task<LoginStatus> Login(object loginData);
        Task<LoginStatus> Login(string username, string password);
    }
}
