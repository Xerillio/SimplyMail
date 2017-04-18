using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimplyMail.Utils.Input
{
    class CommandBase : ICommand
    {
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        private readonly Action<object> _action;

        public CommandBase(Action<object> action)
        {
            _action = action;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
