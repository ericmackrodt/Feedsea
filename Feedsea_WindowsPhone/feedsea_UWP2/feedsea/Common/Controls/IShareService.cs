using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common.Controls
{

   public interface IShareService
   {
      event EventHandler OnShareFinished;

      bool IsShareOpen { get; }

      void Share(ArticleData article);

      void CancelShare();

   }

}