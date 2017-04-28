//
// File: ObservableTask.cs
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
    
    public class ObservableTask : ViewModelBase
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
            set { _prettyErrorMessage = value; RaisePropertyChanged(); }
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
            RaisePropertyChanged("Status");
            RaisePropertyChanged("IsCompleted");
            RaisePropertyChanged("IsNotCompleted");

            if (task.IsCanceled)
            {
                RaisePropertyChanged("IsCanceled");
            }
            else if (task.IsFaulted)
            {
                RaisePropertyChanged("IsFaulted");
                RaisePropertyChanged("Exception");
                RaisePropertyChanged("InnerException");
                RaisePropertyChanged("ErrorMessage");
                if (exceptionPrettifier != null)
                    PrettyErrorMessage = exceptionPrettifier(Exception);
            }
            else
            {
                RaisePropertyChanged("IsSuccessfullyCompleted");
            }
        }
    }

    public class ObservableTask<T> : ObservableTask
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
                RaisePropertyChanged("Result");
        }
    }
}
