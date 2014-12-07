using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Navigation;

namespace feedsea.Common.Controls
{
    public class ChangePropertyOnTap : Behavior<FrameworkElement>
    {
        public object TargetControl
        {
            get { return (string)GetValue(TargetControlProperty); }
            set { SetValue(TargetControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetControlProperty =
            DependencyProperty.Register("TargetControl", typeof(string), typeof(ChangePropertyOnTap), new PropertyMetadata(null, new PropertyChangedCallback(TargetControlPropertyChanged)));

        private static void TargetControlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public object PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(ChangePropertyOnTap), new PropertyMetadata(null, new PropertyChangedCallback(PropertyNamePropertyChanged)));

        private static void PropertyNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public object PropertyValueToSet
        {
            get { return (object)GetValue(PropertyValueToSetProperty); }
            set { SetValue(PropertyValueToSetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyValueToSetProperty =
            DependencyProperty.Register("PropertyValueToSet", typeof(object), typeof(ChangePropertyOnTap), new PropertyMetadata(null, new PropertyChangedCallback(PropertyValueToSetPropertyChanged)));

        private static void PropertyValueToSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Tap += AssociatedObject_Tap;
        }

        void AssociatedObject_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var control = FindVisualChildByName<ContextMenu>(this.AssociatedObject.Parent, (string)TargetControl);
            control.GetType().GetProperty((string)PropertyName).SetValue(control, PropertyValueToSet);
        }

        public T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
    }
}
