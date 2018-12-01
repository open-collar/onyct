// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using JetBrains.Annotations;

// Type overrides Object.Equals(object o) but does not override Object.GetHashCode() - that is up to the derived class.
#pragma warning disable 659,660,661

namespace OpenCollar.Onyct
{
    /// <summary>A base class for objects that support equality and comparison.</summary>
    /// <typeparam name="T">The type of the derived class.</typeparam>
    /// <seealso cref="System.IEquatable{T}" />
    /// <seealso cref="System.IComparable" />
    /// <seealso cref="System.IComparable{T}" />
    public abstract class Comparable<T> : IEquatable<T>, IComparable, IComparable<T> where T : Comparable<T>
    {
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates
        /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance
        /// occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows
        /// <paramref name="obj" /> in the sort order.
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param>
        /// <exception cref="System.ArgumentException"><paramref name="obj" /> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if(ReferenceEquals(obj, null))
            {
                return 1;
            }

            if(ReferenceEquals(obj, this))
            {
                return 0;
            }

            var instance = obj as T;

            if(ReferenceEquals(instance, null))
            {
                throw new ArgumentException($@"'{nameof(obj)}' is not of the same type as this instance.");
            }

            return Compare((T)this, instance);
        }

        /// <summary>Compares the current object with another object of the same type.</summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following
        /// meanings: Value Meaning Less than zero This object is less than the <paramref name="other" /> parameter.Zero This
        /// object is equal to <paramref name="other" />. Greater than zero This object is greater than <paramref name="other" />.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(T other)
        {
            return Compare((T)this, other);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise,
        /// <see langword="false" />.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(T other)
        {
            return Compare((T)this, other) == 0;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to the current
        /// <see cref="System.Object" />.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the specified <see cref="System.Object" /> is equal to the current
        /// <see cref="System.Object" />; otherwise, <see langword="false" />.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if(ReferenceEquals(obj, null))
            {
                return false;
            }

            if(ReferenceEquals(obj, this))
            {
                return true;
            }

            var instance = obj as T;

            if(ReferenceEquals(instance, null))
            {
                return false;
            }

            return Compare((T)this, instance) == 0;
        }

        /// <summary>
        /// Compares two instances of this class and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="first">
        /// The first instance to compare.  Will never be <see langword="null" /> or a reference to
        /// <paramref name="second" />.
        /// </param>
        /// <param name="second">
        /// The second instance to compare.  Will never be <see langword="null" /> or a reference to
        /// <paramref name="first" />.
        /// </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="bullet">
        ///     <item>
        ///         <term>Less Than Zero</term>
        ///         <description><paramref name="first" /> precedes <paramref name="second" /> in the sort order.</description>
        ///     </item> <item>
        ///         <term>Zero</term>
        ///         <description>
        ///         <paramref name="first" /> occurs in the same position in the sort order as
        ///         <paramref name="second" />.
        ///         </description>
        ///     </item> <item>
        ///         <term>More Than Zero</term>
        ///         <description><paramref name="first" /> follows <paramref name="second" /> in the sort order.</description>
        ///     </item>
        /// </list>
        /// </returns>
        private static int Compare(T first, T second)
        {
            if(ReferenceEquals(first, second))
            {
                return 0;
            }

            if(ReferenceEquals(first, null))
            {
                return -1;
            }

            if(ReferenceEquals(second, null))
            {
                return 1;
            }

            return first.OnCompare(first, second);
        }

        /// <summary>
        /// Compares two instances of this class and returns an integer that indicates whether the current instance
        /// precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="first">
        /// The first instance to compare.  Will never be <see langword="null" /> or a reference to
        /// <paramref name="second" />.
        /// </param>
        /// <param name="second">
        /// The second instance to compare.  Will never be <see langword="null" /> or a reference to
        /// <paramref name="first" />.
        /// </param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
        /// <list type="bullet">
        ///     <item>
        ///         <term>Less Than Zero</term>
        ///         <description><paramref name="first" /> precedes <paramref name="second" /> in the sort order.</description>
        ///     </item> <item>
        ///         <term>Zero</term>
        ///         <description>
        ///         <paramref name="first" /> occurs in the same position in the sort order as
        ///         <paramref name="second" />.
        ///         </description>
        ///     </item> <item>
        ///         <term>More Than Zero</term>
        ///         <description><paramref name="first" /> follows <paramref name="second" /> in the sort order.</description>
        ///     </item>
        /// </list>
        /// </returns>
        protected abstract int OnCompare([NotNull] T first, [NotNull] T second);

        /// <summary>Implements the equality operator.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator ==(Comparable<T> left, Comparable<T> right)
        {
            return Compare((T)left, (T)right) == 0;
        }

        /// <summary>Implements the inequality operator.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator !=(Comparable<T> left, Comparable<T> right)
        {
            return Compare((T)left, (T)right) != 0;
        }

        /// <summary>Implements the greater-than operator.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator >(Comparable<T> left, Comparable<T> right)
        {
            return Compare((T)left, (T)right) > 0;
        }

        /// <summary>Implements the less-than operator.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator <(Comparable<T> left, Comparable<T> right)
        {
            return Compare((T)left, (T)right) < 0;
        }

        /// <summary>Implements the greater-than-or-equals operator.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator >=(Comparable<T> left, Comparable<T> right)
        {
            return Compare((T)left, (T)right) >= 0;
        }

        /// <summary>Implements the less-than-or-equals operator.</summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>The result of the operation.</returns>
        public static bool operator <=(Comparable<T> left, Comparable<T> right)
        {
            return Compare((T)left, (T)right) <= 0;
        }
    }
}