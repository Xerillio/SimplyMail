//
// File: WebBrowserHelper.cs
// Author: Casper Sørensen
//
//   Copyright 2017 Casper Sørensen
//	 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//	 
//       http://www.apache.org/licenses/LICENSE-2.0
//	 
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimplyMail.WPF.Views.Helpers
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
