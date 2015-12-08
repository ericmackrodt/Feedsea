using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Interactivity;

namespace feedsea.Common.Controls
{

   public class UpdateTextBoxBinding
      : Behavior<Windows.UI.Xaml.Controls.TextBox>
   {

      protected override void OnAttached()
      {
         base.OnAttached();
         AssociatedObject.KeyUp += AssociatedObject_KeyUp;
      }

      void AssociatedObject_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
      {
         (sender as Windows.UI.Xaml.Controls.TextBox).GetBindingExpression(PhoneTextBox.TextProperty).UpdateSource();
      }

      protected override void OnDetaching()
      {
         base.OnDetaching();
         AssociatedObject.KeyUp -= AssociatedObject_KeyUp;
      }

   }

}