using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public interface IFullLoadingService
    {
        void StartLoading();
        void StartLoading(string message);
        void EndLoading();
    }

    public interface ILoadingService
    {
        void StartLoading();
        void StartLoading(string message);
        void EndLoading();
        void EndLoading(string message);
    }
}
