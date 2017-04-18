using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Utils
{
    static class SafetyChecker
    {
        public static T RequireArgumentNonNull<T>(T value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName);
            else
                return value;
        }

        public static T RequireNonNull<T>(T value)
        {
            if (value == null)
                throw new NullReferenceException();
            else
                return value;
        }

        public static T RequireArgumentType<T>(object obj, string paramName)
        {
            if (!(obj is T))
                throw new UnexpectedArgumentTypeException(typeof(T), paramName);
            else
                return (T)obj;
        }
    }
}
