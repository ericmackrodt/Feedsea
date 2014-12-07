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

namespace feedsea.UserControls
{
    public partial class ValidationMessageControl : UserControl
    {
        public ValidationMessageControl()
        {
            InitializeComponent();
        }

        //public string Text
        //{
        //    get { return (string)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); }
        //}

        //public static readonly DependencyProperty TextProperty =
        //    DependencyProperty.Register(
        //        "Text",
        //        typeof(string),
        //        typeof(ValidationMessageControl),
        //        new PropertyMetadata("", OnTextChanged));

        //private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (e.NewValue == e.OldValue)
        //        return;

        //    var control = (d as ValidationMessageControl);
        //    control.txtMessage.Text = (string)e.NewValue;
        //}

        public ValidationMessage Validation
        {
            get { return (ValidationMessage)GetValue(ValidationProperty); }
            set { SetValue(ValidationProperty, value); }
        }

        public static readonly DependencyProperty ValidationProperty =
            DependencyProperty.Register(
                "ValidationMessage",
                typeof(ValidationMessage),
                typeof(ValidationMessageControl),
                new PropertyMetadata(null, new PropertyChangedCallback(OnValidationChanged)));

        private static void OnValidationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
                return;

            var control = (d as ValidationMessageControl);

            control.RaiseValueChangedEvent(e);
        }

        private void RaiseValueChangedEvent(DependencyPropertyChangedEventArgs args)
        {
            Validation.IsValidChanged += Validation_IsValidChanged;
            Validation.MessageChanged += Validation_MessageChanged;
        }

        void Validation_MessageChanged(object sender, EventArgs e)
        {
            if (Validation == null || string.IsNullOrWhiteSpace(Validation.Message) || Validation.IsValid == true)
                return;

            ClosingAnimation.Begin();
            ClosingAnimation.Completed += (se, ev) =>
            {
                txtMessage.Text = Validation.Message;
                OpeningAnimation.Begin();
            };
        }

        void Validation_IsValidChanged(object sender, EventArgs e)
        {
            if (Validation == null)
                return;

            if (Validation.IsValid == true)
            {
                ClosingAnimation.Begin();
                ClosingAnimation.Completed += (se, ev) =>
                {
                    ContractingAnimation.Begin();
                    ContractingAnimation.Completed += (snd, earg) =>
                    {
                        Visibility = Visibility.Collapsed;
                    };
                };
                return;
            }
            else
            {
                Visibility = System.Windows.Visibility.Visible;
                ExpandingAnimation.Begin();
                ExpandingAnimation.Completed += (snd, ev) =>
                {
                    txtMessage.Text = Validation.Message;
                    OpeningAnimation.Begin();
                };
            }
        }
    }
}
