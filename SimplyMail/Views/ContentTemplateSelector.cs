using SimplyMail.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimplyMail.Views
{
    class ContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate HomeTemplate { get; set; }
        public DataTemplate LoginTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = null;

            if (item is Home)
                template = HomeTemplate;
            else if (item is Login)
                template = LoginTemplate;
            
            return template ?? base.SelectTemplate(item, container);
        }
    }
}
