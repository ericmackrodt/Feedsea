using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedsea.Common.Services
{
    public interface IMessageBoxService
    {
        Task<bool> ConfirmationBox(string resourceString);
        Task InformationBox(string resourceString);
    }
}
