//
// File: Home.cs
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
using SimplyMail.Utils;
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

        ObservableTask<ObservableCollection<MailMessage>> _currentFolderMessagesTask;
        public ObservableTask<ObservableCollection<MailMessage>> CurrentFolderMessagesTask
        {
            get { return _currentFolderMessagesTask; }
            set { _currentFolderMessagesTask = value; OnPropertyChanged("CurrentFolderMessagesTask"); }
        }

        public ICommand AddAccountCommand => new CommandBase(OnAddAccount);

        public Home(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
            MailFolder.FolderSelected += MailFolder_FolderSelected;
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

        private void MailFolder_FolderSelected(object sender, EventArgs e)
        {
            var folder = SafetyChecker.RequireArgumentType<MailFolder>(sender, "sender");
            CurrentFolderMessagesTask =
                new ObservableTask<ObservableCollection<MailMessage>>(folder.GetMessages());
        }
    }
}
