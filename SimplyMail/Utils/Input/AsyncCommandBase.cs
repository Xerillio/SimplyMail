using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Utils.Input
{
    class AsyncCommandBase : IAsyncCommand
    {
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        private readonly Func<object, Task> _function;

        public AsyncCommandBase(Func<object, Task> function)
        {
            _function = function;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        public Task ExecuteAsync(object parameter)
        {
            return _function(parameter);
        }
    }
}
