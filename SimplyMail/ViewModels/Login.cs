//
// File: Login.cs
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
using SimplyMail.Middleware;
using SimplyMail.Models;
using SimplyMail.Utils;
using SimplyMail.ViewModels.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.ViewModels
{
    public class Login : ViewModelBase
    {
        public event EventHandler<MailAccount> LoginSucceeded;

        public string Email { get; set; }

        ObservableTask _loginTask;
        public ObservableTask LoginTask
        {
            get { return _loginTask; }
            set { _loginTask = value; RaisePropertyChanged(); }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand<string>(pwd =>
                    LoginTask = new ObservableTask(OnLogin(pwd), OnLoginFailed)
                );
            }
        }

        private async Task OnLogin(string password)
        {
            // TODO check if username and password has been filled
            string email = Email;

            // TODO only for quick test
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
            {
                try
                {
                    var lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(
                    new System.IO.DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.Parent.FullName,
                    "super-secret-folder\\credentials.properties"));
                    foreach (var line in lines)
                    {
                        var splits = line.Split('=');
                        switch (splits[0].ToLower())
                        {
                            case "email":
                                email = splits[1];
                                break;
                            case "password":
                                password = splits[1];
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch { }
            }

            var service = new ImapService();
            await service.LoginAsync(email, password);

            LoginSucceeded?.Invoke(this, new MailAccount(email, service));
        }

        private string OnLoginFailed(AggregateException ex)
        {
            if (!SimpleIoc.Default.IsRegistered<IResources>())
                return null;
            foreach (var exception in ex.InnerExceptions)
            {
                if (exception is MailKit.Security.AuthenticationException)
                    return SimpleIoc.Default.GetInstance<IResources>()?.InvalidCredentialsMessage;
            }
            return null;
        }
    }
}
