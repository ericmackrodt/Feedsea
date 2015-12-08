using feedsea.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{

   public class NewsEventArgs<T>
      : EventArgs
   {

      public T Object { get; set; }


      public NewsEventArgs(T obj)
      {
         Object = obj;
      }
   }

}