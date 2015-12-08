using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.BackgroundAgent.Common
{

   public enum ExceptionType
   {NoNetworkAccess}

   public class TileCreationException
      : Exception
   {

      public ExceptionType Type { get; set; }


      public TileCreationException(ExceptionType type)
      : base()
      {
         Type = type;
      }

      public TileCreationException(string message, ExceptionType type)
      : base(message)
      {
         Type = type;
      }
   }

}