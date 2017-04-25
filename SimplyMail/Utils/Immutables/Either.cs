//
// File: Either.cs
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

namespace SimplyMail.Utils.Immutables
{
    struct Either<L, R>
    {
        Optional<L> _left;
        Optional<R> _right;

        public static Either<L, R> Left(L value)
        {
            return new Either<L, R>(Optional<L>.From(value), Optional<R>.Empty());
        }

        public static Either<L, R> Right(R value)
        {
            return new Either<L, R>(Optional<L>.Empty(), Optional<R>.From(value));
        }

        Either(Optional<L> lOpt, Optional<R> rOpt)
        {
            _left = lOpt;
            _right = rOpt;
        }

        public T Select<T>(Func<L, T> lf, Func<R, T> rf)
        {
            var thisRight = _right;
            return _left.Select(lf).OrElseGet(() => thisRight.Select(rf).Value);
        }

        public Either<L2, R> SelectLeft<L2>(Func<L, L2> lf)
        {
            return new Either<L2, R>(_left.Select(lf), _right);
        }

        public Either<L, R2> SelectRight<R2>(Func<R, R2> rf)
        {
            return new Either<L, R2>(_left, _right.Select(rf));
        }

        public void Apply(Action<L> lAction, Action<R> rAction)
        {
            _left.IfPresent(lAction);
            _right.IfPresent(rAction);
        }
    }
}
