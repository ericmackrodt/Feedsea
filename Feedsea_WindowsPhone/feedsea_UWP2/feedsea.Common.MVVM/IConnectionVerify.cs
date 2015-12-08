using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.MVVM
{

   public interface IConnectionVerify
   {

      bool HasInternetConnection();

      void ShowNoConnectionMessage();

      bool VerifyConnectionException(Exception ex);

   }

}