using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using feedsea.Common;
using System.Windows.Controls.Primitives;

namespace feedsea.UserControls
{
    public partial class MobilizerMenuControl : UserControl
    {
        public event EventHandler<Mobilizers> MobilizerSelected;

        public Mobilizers MobilizerSelectedProperty
        {
            get { return (Mobilizers)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MobilizerSelected", typeof(Mobilizers), typeof(MobilizerMenuControl), new PropertyMetadata(Mobilizers.Page));

        public MobilizerMenuControl()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MobilizerSelectedProperty = (Mobilizers)MobilizersList.SelectedIndex;
            if (MobilizerSelected != null)
                MobilizerSelected(this, MobilizerSelectedProperty);
            MobilizersList.SelectedIndex = -1;
        }

        public void Close()
        {
            var parent = (this.Parent as Popup);

            if (parent == null)
            {
                return;
            }

            ClosingAnimation.Completed += (s, e) =>
            {
                parent.IsOpen = false;
            };
            ClosingAnimation.Begin();
        }

        public void PopupOpenEvent()
        {
            var parent = (this.Parent as Popup);
            if (parent == null)
            {
                LayoutRoot.Opacity = 1;
                return;
            }

            parent.Opened += (s, e) => { OpeningAnimation.Begin(); };
        }
    }
}
