using SimplyMail.ViewModels.Input;
using SimplyMail.ViewModels.Mail;
using SimplyMail.Views.Middleware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.ViewModels
{
    class Home : ObservableObject
    {
        IWindowFactory _windowFactory;

        ObservableCollection<MailAccount> _mailAccounts = new ObservableCollection<MailAccount>();
        public ObservableCollection<MailAccount> MailAccounts => _mailAccounts;

        public ICommand AddAccountCommand => new CommandBase(OnAddAccount);

        public Home(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        private void OnAddAccount(object obj)
        {
            var loginVm = new Login();
            loginVm.LoginSucceeded += (s, acc) =>
            {
                MailAccounts.Add(acc);
            };
            _windowFactory.CreateWindow(loginVm);
        }
    }
}
