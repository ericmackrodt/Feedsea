using feedsea.Common.MVVM;
using feedsea.Common.MVVM.Commands;
using feedsea.Common.Providers;
using feedsea.Common.Providers.Data;
using feedsea.Common.Providers.Feedly;
using feedsea.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace feedsea.ViewModels
{

   [DataContract]
   public class SelectedSourceViewModel
      : AViewModel<SelectedSourceViewModel>
   {
      private INewsService newsService;

      public INewsSource SelectedSource
      {
         get
         {
            return newsService.SelectedSource;
         }
         set
         {
            newsService.SelectedSource = value;
         }
      }

      private ICommand clearSelectedSourceCommand;

      public ICommand ClearSelectedSourceCommand
      {
         get
         {
            return clearSelectedSourceCommand;
         }
         set
         {
            clearSelectedSourceCommand = value;
         }
      }


      public SelectedSourceViewModel(INewsService newsService, IConnectionVerify connectionVerify)
      {
         this.connection = connectionVerify;
         this.newsService = newsService;
         this.newsService.PropertyChanged += newsService_PropertyChanged;
         this.clearSelectedSourceCommand = new AsyncDelegateCommand<INewsSource>(o => ConnectionVerifyCall(ClearSelectedSource, o, OnConnectionFail));
      }

      private void OnConnectionFail()
      {
         newsService.NotifyFail();
      }

      private async Task ClearSelectedSource(INewsSource arg)
      {
         await newsService.ClearSelectedSource();
      }

      private void newsService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
      {
         NotifyChanged(e.PropertyName);
      }

   }

}