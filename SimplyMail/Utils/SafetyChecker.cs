//
// File: SafetyChecker.cs
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
