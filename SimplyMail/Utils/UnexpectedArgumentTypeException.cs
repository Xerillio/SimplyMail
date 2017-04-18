using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Utils
{
    class UnexpectedArgumentTypeException : Exception
    {
        public Type ExpectedType { get; }

        public UnexpectedArgumentTypeException(Type type)
            : base($"Argument not of type '{type.Name}'")
        {
            ExpectedType = type;
        }

        public UnexpectedArgumentTypeException(Type type, string paramName)
            : base($"Argument '{paramName}' not of type '{type.Name}'")
        {
            ExpectedType = type;
        }
    }
}
