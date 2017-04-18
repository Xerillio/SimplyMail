using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.ViewModels
{
    /// <summary>
    /// From: https://msdn.microsoft.com/en-us/magazine/dn605875.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    
    class ObservableTask : ObservableObject
    {
        public Task Task { get; }
        public TaskStatus Status => Task.Status;
        public bool IsCompleted => Task.IsCompleted;
        public bool IsNotCompleted => !IsCompleted;
        public bool IsSuccessfullyCompleted => Task.Status == TaskStatus.RanToCompletion;
        public bool IsCanceled => Task.IsCanceled;
        public bool IsFaulted => Task.IsFaulted;
        public AggregateException Exception => Task.Exception;
        public Exception InnerException => Exception?.InnerException;
        public string ErrorMessage => InnerException?.Message;
        string _prettyErrorMessage;
        public string PrettyErrorMessage
        {
            get { return _prettyErrorMessage; }
            set { _prettyErrorMessage = value; OnPropertyChanged("PrettyErrorMessage"); }
        }

        public ObservableTask(Task task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        public ObservableTask(Task task, Func<AggregateException, string> exceptionPrettifier)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task, exceptionPrettifier);
            }
        }

        private async Task WatchTaskAsync(Task task, Func<AggregateException, string> exceptionPrettifier = null)
        {
            try
            {
                await task;
            }
            catch
            {
            }

            // Notify task has completed
            OnPropertyChanged("Status");
            OnPropertyChanged("IsCompleted");
            OnPropertyChanged("IsNotCompleted");

            if (task.IsCanceled)
            {
                OnPropertyChanged("IsCanceled");
            }
            else if (task.IsFaulted)
            {
                OnPropertyChanged("IsFaulted");
                OnPropertyChanged("Exception");
                OnPropertyChanged("InnerException");
                OnPropertyChanged("ErrorMessage");
                if (exceptionPrettifier != null)
                    PrettyErrorMessage = exceptionPrettifier(Exception);
            }
            else
            {
                OnPropertyChanged("IsSuccessfullyCompleted");
            }
        }
    }

    class ObservableTask<T> : ObservableTask
    {
        public new Task<T> Task { get; private set; }
        public T Result => IsSuccessfullyCompleted ? Task.Result : default(T);

        public ObservableTask(Task<T> task) : base(task)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        public ObservableTask(Task<T> task, Func<AggregateException, string> exceptionPrettifier)
            : base(task, exceptionPrettifier)
        {
            Task = task;
            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task, Func<AggregateException, string> exceptionPrettifier = null)
        {
            try
            {
                await task;
            }
            catch
            {
            }

            // Notify task has completed
            OnPropertyChanged("Status");
            OnPropertyChanged("IsCompleted");
            OnPropertyChanged("IsNotCompleted");

            if (task.IsCanceled)
            {
                OnPropertyChanged("IsCanceled");
            }
            else if (task.IsFaulted)
            {
                OnPropertyChanged("IsFaulted");
                OnPropertyChanged("Exception");
                OnPropertyChanged("InnerException");
                OnPropertyChanged("ErrorMessage");
                if (exceptionPrettifier != null)
                    PrettyErrorMessage = exceptionPrettifier(Exception);
            }
            else
            {
                OnPropertyChanged("IsSuccessfullyCompleted");
                OnPropertyChanged("Result");
            }
        }
    }
}
