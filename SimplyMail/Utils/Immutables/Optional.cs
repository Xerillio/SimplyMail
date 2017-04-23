using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Utils.Immutables
{
    struct Optional<T>
    {
        static readonly Optional<T> EMPTY = new Optional<T>();

        public bool HasValue
        {
            get { return _value != null; }
        }

        T _value;
        public T Value
        {
            get
            {
                if (HasValue)
                    return _value;
                else
                    throw new InvalidOperationException("No value present.");
            }
        }

        public static Optional<T> Empty()
        {
            return EMPTY;
        }

        public static Optional<T> From(T value)
        {
            SafetyChecker.RequireArgumentNonNull(value, "value");
            return new Optional<T>
            {
                _value = value
            };
        }

        public static Optional<T> FromNullable(T value)
        {
            if (value == null)
                return Empty();
            else
                return From(value);
        }

        public static explicit operator T(Optional<T> opt)
        {
            return opt.Value;
        }

        public static implicit operator Optional<T>(T value)
        {
            return From(value);
        }

        public Optional<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            SafetyChecker.RequireArgumentNonNull(selector, "selector");
            return HasValue ?
                Optional<TResult>.From(selector(Value)) :
                Optional<TResult>.Empty();
        }

        public async Task<Optional<TResult>> SelectAsync<TResult>(Func<T, Task<TResult>> selector)
        {
            SafetyChecker.RequireArgumentNonNull(selector, "selector");
            if (HasValue)
            {
                var newVal = await selector(Value);
                return Optional<TResult>.From(newVal);
            }
            else
            {
                return Optional<TResult>.Empty();
            }
        }

        //public AsyncOptional<TResult> SelectAsync<TResult>(Func<T, Task<TResult>> selector)
        //{
        //    Utils.RequireArgumentNonNull(selector, "selector");
        //    return HasValue ?
        //        AsyncOptional<TResult>.From(selector(Value)) :
        //        AsyncOptional<TResult>.Empty();
        //}

        public Optional<TResult> SelectMany<TResult>(Func<T, Optional<TResult>> selector)
        {
            SafetyChecker.RequireArgumentNonNull(selector, "selector");
            return HasValue ?
                SafetyChecker.RequireNonNull(selector(Value)) :
                Optional<TResult>.Empty();
        }

        public Optional<T> Where(Func<T, bool> predicate)
        {
            SafetyChecker.RequireArgumentNonNull(predicate, "predicate");
            return HasValue && predicate(Value) ?
                this :
                Empty();
        }

        public void IfPresent(Action<T> action)
        {
            if (HasValue)
                action(Value);
        }

        public void Apply(Action<T> presentAction, Action absentAction)
        {
            if (HasValue)
                presentAction(Value);
            else
                absentAction();
        }

        public T OrElse(T other)
        {
            return HasValue ? Value : other;
        }

        public T OrElseGet(Func<T> producer)
        {
            return HasValue ? Value : producer();
        }

        public override bool Equals(object obj)
        {
            if (obj is Optional<T>)
                return Equals((Optional<T>)obj);
            else
                return false;
        }
        public bool Equals(Optional<T> other)
        {
            if (HasValue && other.HasValue)
                return Equals(Value, other.Value);
            else
                return HasValue == other.HasValue;
        }

        public override int GetHashCode()
        {
            int hash = HasValue.GetHashCode();
            if (HasValue)
                hash = hash * 17 + Value.GetHashCode();
            return hash;
        }
    }
}
