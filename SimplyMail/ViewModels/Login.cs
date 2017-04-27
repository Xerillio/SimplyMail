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
using SimplyMail.Models;
using SimplyMail.Utils;
using SimplyMail.ViewModels.Input;
using SimplyMail.ViewModels.Mail;
using SimplyMail.ViewModels.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.ViewModels
{
    public class Login : ObservableObject
    {
        public event EventHandler<MailAccount> LoginSucceeded;

        public string Username { get; set; }

        ObservableTask _loginTask;
        public ObservableTask LoginTask
        {
            get { return _loginTask; }
            set { _loginTask = value;  OnPropertyChanged("LoginTask"); }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new CommandBase(obj =>
                    LoginTask = new ObservableTask(OnLogin(obj), OnLoginFailed)
                );
            }
        }

        private async Task OnLogin(object obj)
        {
            var objs = SafetyChecker.RequireArgumentType<object[]>(obj, "obj");

            // TODO check if username and password has been filled
            string email = Username,
                password = SafetyChecker.RequireNonNull(objs[0] as string);

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
            if (objs.Length > 1)
            {
                var completable = SafetyChecker.RequireNonNull(objs[1] as ICompletable);
                completable.OnCompleted();
            }
        }

        private string OnLoginFailed(AggregateException ex)
        {
            foreach (var exception in ex.InnerExceptions)
            {
                if (exception is MailKit.Security.AuthenticationException)
                    return IoC.Instance.Resources?.InvalidCredentialsMessage;
            }
            return null;
        }
    }
}
