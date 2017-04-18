using SimplyMail.Models;
using SimplyMail.Utils;
using SimplyMail.Utils.Input;
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
            var pwBox = SafetyChecker.RequireArgumentType<PasswordBox>(obj, "obj");
            // TODO check if username and password has been filled
            var service = new ImapService();

            // TODO only for quick test
            if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(pwBox.Password))
            {
                string username = "", password = "";
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
                            case "username":
                                username = splits[1];
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
                await service.LoginAsync(username, password).ConfigureAwait(false);
            }
            else
            {
                await service.LoginAsync(Username, pwBox.Password).ConfigureAwait(false);
            }

            Main.CurrentInstance.MainContent = new Home(service);
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
