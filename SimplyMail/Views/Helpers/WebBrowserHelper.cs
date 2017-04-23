using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimplyMail.Views.Helpers
{
    class WebBrowserHelper
    {
        public static string GetHtmlString(WebBrowser obj)
        {
            return (string)obj.GetValue(HtmlStringProperty);
        }

        public static void SetHtmlString(WebBrowser obj, string value)
        {
            obj.SetValue(HtmlStringProperty, value);
        }

        // Using a DependencyProperty as the backing store for HtmlString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlStringProperty =
            DependencyProperty.RegisterAttached(
                "HtmlString",
                typeof(string),
                typeof(WebBrowserHelper),
                new PropertyMetadata(OnHtmlStringChanged));

        private static void OnHtmlStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = d as WebBrowser;
            if (d != null)
                webBrowser.NavigateToString(e.NewValue as string ?? "&nbsp;");
        }
    }
}
