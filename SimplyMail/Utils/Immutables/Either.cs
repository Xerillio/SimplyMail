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
