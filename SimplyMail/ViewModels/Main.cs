using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyMail.ViewModels
{
    class Main : ObservableObject
    {
        static Main _currentInstance;
        public static Main CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                    _currentInstance = new Main();
                return _currentInstance;
            }
        }

        private object _mainContent;

        public object MainContent
        {
            get { return _mainContent; }
            set
            {
                if (_mainContent == value)
                    return;
                _mainContent = value;
                OnPropertyChanged("MainContent");
            }
        }

        Main()
        {
            MainContent = new Login();
        }
    }
}
