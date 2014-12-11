using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{
    public interface IFullLoadingService
    {
        bool IsLoading { get; }
        void StartLoading();
        void StartLoading(string message);
        void EndLoading();
    }

    public interface ILoadingService
    {
        bool IsLoading { get; }
        void StartLoading();
        void StartLoading(string message);
        void EndLoading();
        void EndLoading(string message);
    }
}
