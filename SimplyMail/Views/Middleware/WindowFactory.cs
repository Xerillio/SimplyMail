//
// File: WindowFactory.cs
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
using SimplyMail.ViewModels;
using SimplyMail.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyMail.Views.Middleware
{
    class WindowFactory : IWindowFactory
    {
        public void CreateWindow<T>(T windowViewModel)
        {
            Window window = null;
            if (windowViewModel is Login)
                window = new PopupWindow();

            if (window != null)
            {
                window.DataContext = windowViewModel;
                window.ShowDialog();
            }
        }
    }
}
