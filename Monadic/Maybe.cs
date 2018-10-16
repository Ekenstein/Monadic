using System;
using System.Collections.Generic;
using System.Linq;
using Monadic.Extensions;

namespace Monadic
{
    public static class Maybe
    {
        public static Maybe<T> Just<T>(T t) => Maybe<T>.Just(t);

        public static Maybe<T> Create<T>(T? t)
            where T : struct => t == null ? Maybe<T>.Nothing : Just(t.Value);

        public static Maybe<T> Create<T>(T t)
        {
            return t == null ? Maybe<T>.Nothing : Just(t);
        }
    }

    /// <summary>
    /// The Maybe type encapsulates an optional value. A value of type <see cref="Maybe{T}"/> either contains a value of type <see cref="T"/> 
    /// (represented as Just <see cref="T"/>), or it is empty (represented as Nothing).
    /// </summary>
    /// <typeparam name="T">The type of value the Maybe is wrapping.</typeparam>
    public struct Maybe<T>
    {
        private readonly IEnumerable<T> _item;

        public static Maybe<T> Just(T t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t));
            }

            return new Maybe<T>(new [] { t });
        }

        public static Maybe<T> Nothing => new Maybe<T>(new T[0]);

        private Maybe(IEnumerable<T> item)
        {
            _item = item;
        }

        public T Value => IsJust
            ? _item.Single()
            : throw new InvalidOperationException();

        public bool IsNothing => _item == null || !_item.Any();
        public bool IsJust => !IsNothing;

        public static implicit operator Maybe<T>(T t) => t != null
            ? Just(t)
            : Nothing;

        public static implicit operator T(Maybe<T> maybe) => maybe.IsNothing
            ? default(T)
            : maybe.Value;

        public override string ToString() => this.FromMaybe("Nothing", v => $"Just ({v})");
    }
}
