using feedsea.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Controls
{

   public interface IXAuthLogin
   {

      bool IsOpen { get; }

      Task<bool> Login(IProvider provider);

      void Cancel();

   }

}