using System;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using System.Windows.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace feedsea.Common.MVVM
{

   public abstract class ADataTemplateSelector
      : Windows.UI.Xaml.Controls.ContentControl
   {

      public virtual DataTemplate SelectTemplate(object item, Windows.UI.Xaml.DependencyObject container)
      {
         return null;
      }

      protected override void OnContentChanged(object oldContent, object newContent)
      {
         base.OnContentChanged(oldContent, newContent);
         ContentTemplate = SelectTemplate(newContent, this);
      }

   }

}