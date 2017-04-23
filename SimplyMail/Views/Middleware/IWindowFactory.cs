using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Views.Middleware
{
    interface IWindowFactory
    {
        void CreateWindow<T>(T windowViewModel);
    }
}
