using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Controls
{

   public interface IOAuthLogin
   {

      bool IsOpen { get; }

      Task<string> Login(string loginUrl, string redirectUrl);

      void Cancel();

   }

}