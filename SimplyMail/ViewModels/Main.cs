﻿//
// File: Main.cs
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
using System.Windows;

namespace SimplyMail.ViewModels
{
    public class Main : ViewModelBase
    {
        static Main _currentInstance;
        public static Main CurrentInstance => _currentInstance ?? (_currentInstance = new Main());

        private object _mainContent;

        public object MainContent
        {
            get { return _mainContent; }
            set
            {
                if (_mainContent == value)
                    return;
                _mainContent = value;
                RaisePropertyChanged();
            }
        }

        public Main()
        {
            MainContent = new Home();
        }
    }
}
