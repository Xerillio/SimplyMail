using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Utils.Immutables
{
    struct AsyncOptional<T>
    {
        static readonly AsyncOptional<T> EMPTY = new AsyncOptional<T>();

        public bool HasTask
        {
            get { return _valueTask != null; }
        }

        Task<T> _valueTask;
        public Task<T> ValueTask
        {
            get
            {
                if (HasTask)
                    return _valueTask;
                else
                    throw new InvalidOperationException("No task present.");
            }
        }

        public static AsyncOptional<T> Empty()
        {
            return EMPTY;
        }

        public static AsyncOptional<T> From(Task<T> task)
        {
            SafetyChecker.RequireArgumentNonNull(task, "task");
            return new AsyncOptional<T>
            {
                _valueTask = task
            };
        }

        public AsyncOptional<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            var task = _valueTask;
            Func<Task<TResult>> f = async () => selector(await task);
            return AsyncOptional<TResult>.From(f());
        }



        //public static Optional<T> FromNullable(T value)
        //{
        //    if (value == null)
        //        return Empty();
        //    else
        //        return From(value);
        //}

        //public static explicit operator T(Optional<T> opt)
        //{
        //    return opt.Value;
        //}

        //public static implicit operator Optional<T>(T value)
        //{
        //    return From(value);
        //}

        //public AsyncOptional<TResult> Select<TResult>(Func<T, TResult> selector)
        //{
        //    Utils.RequireArgumentNonNull(selector, "selector");
        //    return HasValue ?
        //        AsyncOptional<TResult>.From(selector(ValueTask)) :
        //        AsyncOptional<TResult>.Empty();
        //}

        //public async Task<AsyncOptional<TResult>> SelectAsync<TResult>(Func<T, Task<TResult>> selector)
        //{
        //    Utils.RequireArgumentNonNull(selector, "selector");
        //    return HasValue ?
        //        Optional<TResult>.From(await selector(ValueTask)) :
        //        Optional<TResult>.Empty();
        //}

        //public Optional<TResult> SelectMany<TResult>(Func<T, Optional<TResult>> selector)
        //{
        //    Utils.RequireArgumentNonNull(selector, "selector");
        //    return HasValue ?
        //        Utils.RequireNonNull(selector(ValueTask)) :
        //        Optional<TResult>.Empty();
        //}

        //public Optional<T> Where(Func<T, bool> predicate)
        //{
        //    Utils.RequireArgumentNonNull(predicate, "predicate");
        //    return HasValue && predicate(ValueTask) ?
        //        this :
        //        Empty();
        //}

        public async Task IfPresent(Action<T> action)
        {
            if (HasTask)
                Optional<T>.From(await ValueTask)
                    .IfPresent(action);
        }

        //public void Apply(Action<T> presentAction, Action absentAction)
        //{
        //    if (HasValue)
        //        presentAction(ValueTask);
        //    else
        //        absentAction();
        //}

        //public T OrElse(T other)
        //{
        //    return HasValue ? ValueTask : other;
        //}

        //public T OrElseGet(Func<T> producer)
        //{
        //    return HasValue ? ValueTask : producer();
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is Optional<T>)
        //        return Equals((Optional<T>)obj);
        //    else
        //        return false;
        //}
        //public bool Equals(Optional<T> other)
        //{
        //    if (HasValue && other.HasValue)
        //        return Equals(ValueTask, other.Value);
        //    else
        //        return HasValue == other.HasValue;
        //}
    }
}
