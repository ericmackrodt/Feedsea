using Coding4Fun.Toolkit.Controls;
using feedsea.Common.MVVM.Services;
using feedsea.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feedsea.Common
{

   public class ToastService
      : IToastService
   {

      public void Show(string message)
      {
         ToastPrompt toast = new ToastPrompt();
         toast.Message = message;
         toast.TextWrapping = Windows.UI.Xaml.TextWrapping.Wrap;
         toast.Title = AppResources.ApplicationTitle;
         toast.Show();
      }

   }

}