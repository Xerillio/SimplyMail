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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SimplyMail.Utils;
using SimplyMail.ViewModels.Mail;
using SimplyMail.Middleware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.ViewModels
{
    public class Home : ViewModelBase
    {
        ObservableCollection<MailAccount> _mailAccounts = new ObservableCollection<MailAccount>();
        public ObservableCollection<MailAccount> MailAccounts => _mailAccounts;

        ObservableTask<ObservableCollection<MailMessage>> _currentFolderMessagesTask;
        public ObservableTask<ObservableCollection<MailMessage>> CurrentFolderMessagesTask
        {
            get { return _currentFolderMessagesTask; }
            set { _currentFolderMessagesTask = value; RaisePropertyChanged(); }
        }

        public ICommand AddAccountCommand => new RelayCommand(OnAddAccount);

        public Home()
        {
            MailFolder.FolderSelected += MailFolder_FolderSelected;
        }

        private void OnAddAccount()
        {
            if (!SimpleIoc.Default.IsRegistered<IWindowFactory>())
                return;

            var loginVm = new Login();
            var window = SimpleIoc.Default.GetInstance<IWindowFactory>().CreateWindow(loginVm);
            if (window == null)
                return;

            loginVm.LoginSucceeded += (s, acc) =>
            {
                window.Deactivate();
                MailAccounts.Add(acc);
            };
            window.Activate();
        }

        private void MailFolder_FolderSelected(object sender, EventArgs e)
        {
            var folder = SafetyChecker.RequireArgumentType<MailFolder>(sender, "sender");
            CurrentFolderMessagesTask =
                new ObservableTask<ObservableCollection<MailMessage>>(folder.GetMessages());
        }
    }
}
