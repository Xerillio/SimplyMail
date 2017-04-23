using SimplyMail.ViewModels;
using SimplyMail.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyMail.Views.Middleware
{
    class WindowFactory : IWindowFactory
    {
        public void CreateWindow<T>(T windowViewModel)
        {
            Window window = null;
            if (windowViewModel is Login)
                window = new PopupWindow();

            if (window != null)
            {
                window.DataContext = windowViewModel;
                window.ShowDialog();
            }
        }
    }
}
