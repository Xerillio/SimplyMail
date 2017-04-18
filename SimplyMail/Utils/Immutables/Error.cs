using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Utils.Immutables
{
    class Error
    {
        string _message;
        public string Message => _message;

        public Error(string message)
        {
            _message = message;
        }
    }
}
