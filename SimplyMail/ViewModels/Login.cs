using SimplyMail.Models;
using SimplyMail.Utils;
using SimplyMail.ViewModels.Input;
using SimplyMail.ViewModels.Mail;
using SimplyMail.Views.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimplyMail.ViewModels
{
    class Login : ObservableObject
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
            var pwBox = SafetyChecker.RequireNonNull(objs[0] as PasswordBox);

            // TODO check if username and password has been filled
            var service = new ImapService();
            string email = Username,
                password = pwBox.Password;

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
                    return Properties.Resources.InvalidCredentials;
            }
            return null;
        }
    }
}
