using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Models
{

   public class SettingItemModel
   {

      public string Title { get; set; }

      public string Description { get; set; }

      public string NavigationUri { get; set; }

      public ItemType Type { get; set; }

   }


   public enum ItemType
   {AppPage,
   WebPage,
   Email,
   Purchase,
   Logoff}
}