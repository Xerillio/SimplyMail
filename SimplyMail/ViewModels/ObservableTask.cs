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
        protected Task Task { get; }
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

        public ObservableTask(Task task) : this(task, null)
        { }

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
            OnTaskCompleted(task, exceptionPrettifier);
        }

        protected virtual void OnTaskCompleted(Task task, Func<AggregateException, string> exceptionPrettifier = null)
        {
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
        public T Result => IsSuccessfullyCompleted ? (Task as Task<T>).Result : default(T);

        public ObservableTask(Task<T> task) : this(task, null)
        { }

        public ObservableTask(Task<T> task, Func<AggregateException, string> exceptionPrettifier)
            : base(task, exceptionPrettifier)
        { }

        protected override void OnTaskCompleted(Task task, Func<AggregateException, string> exceptionPrettifier = null)
        {
            base.OnTaskCompleted(task, exceptionPrettifier);
            if (IsSuccessfullyCompleted)
                OnPropertyChanged("Result");
        }
    }
}
