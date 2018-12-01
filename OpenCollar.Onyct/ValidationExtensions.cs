// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>Methods used to validate arguments passed to public functions.</summary>
    public static class ValidationExtensions
    {
        /// <summary>Asserts that <paramref name="value" /> given is a valid instance of the enumerable type</summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The value given was not a valid member of the
        /// <typeparamref name="T" /> type.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value given was not an enumerable type.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">The value given is set to 'Unknown'.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// The value of <paramref name="validationType" /> is not a valid
        /// member of the <see cref="EnumIs" /> type.
        /// </exception>
        [AssertionMethod]
        public static void Validate<T>(this T value, [InvokerParameterName] [NotNull] string argumentName, EnumIs validationType) where T : struct
        {
            switch(validationType)
            {
                case EnumIs.Member:
                case EnumIs.NonZeroMember:
                    var enumType = typeof(T);

                    if(!enumType.IsEnum)
                    {
                        throw new ArgumentOutOfRangeException("value", value, "'value' is not a enum type.");
                    }

                    if(!Enum.IsDefined(enumType, value))
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value,
                                                              string.Format(CultureInfo.InvariantCulture, "'{0}' is not a valid member of the [{1}] type.", argumentName, enumType.FullName));
                    }

                    if(validationType == EnumIs.NonZeroMember)
                    {
                        if(value is Enum)
                        {
                            var type = Enum.GetUnderlyingType(typeof(T));

                            var integerValue = System.Convert.ChangeType(value, type);

                            var e = System.Convert.ToInt64(integerValue);

                            if(e.Equals(0))
                            {
                                throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is set to 'Unknown'.", argumentName));
                            }
                        }
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException("validationType", validationType, string.Format("'validationType' is not a valid member of the [{0}] type.", typeof(EnumIs).FullName));
            }
        }

        /// <summary>Checks that the <paramref name="value" /> given is not <see langword="null" /> and returns it.</summary>
        /// <typeparam name="T">The type of the object to check</typeparam>
        /// <param name="value">The value to check and return.</param>
        /// <returns>The value given, guaranteed to not be <see langword="null" />.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        [NotNull]
        [PublicAPI]
        [AssertionMethod]
        public static T NotNull<T>([CanBeNull] this T value) where T : class
        {
            value.Validate(nameof(value), ObjectIs.NotNull);

            return value;
        }

        /// <summary>
        /// Asserts that the enumerable <paramref name="values" /> and the items it contains meet the criteria specified
        /// in <paramref name="argumentValidationType" /> and throws an exception if it does not.
        /// </summary>
        /// <param name="values">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="values" />.</param>
        /// <param name="argumentValidationType">The type of validation to perform on the argument.</param>
        /// <param name="elementValidationType">The type of validation to perform on the elements in the enumerable argument.</param>
        /// <exception cref="System.ArgumentNullException">The value given was <see langword="null" />.</exception>
        /// <exception cref="System.ArgumentException">Item in the argument given is <see langword="null" />.</exception>
        [AssertionMethod]
        [NotNull]
        public static T[] Validate<T>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [CanBeNull] [ValidatedNotNull]
                                                 this IEnumerable<T> values, [InvokerParameterName] [NotNull] string argumentName, ObjectIs argumentValidationType,
                                                 ElementIs elementValidationType) where T : class
        {
            if(ReferenceEquals(values, null))
            {
                if((argumentValidationType & ObjectIs.NotNull) == ObjectIs.NotNull)
                {
                    throw new ArgumentNullException(argumentName, ErrorMessages.ArgumentNull(argumentName));
                }

                return new T[0];
            }

            if((elementValidationType & ElementIs.NotNull) != ElementIs.NotNull)
            {
                return values.ToArray();
            }

            var results = values.ToArray();
            for(var n = 0; n < results.Length; ++n)
            {
                if(ReferenceEquals(results[n], null))
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, @"Item {0} in '{1}' is null.", n, argumentName), argumentName);
                }
            }

            return results;
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> meets the criteria specified in
        /// <paramref name="argumentValidationType" /> and throws an exception if it does not.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="argumentValidationType">The type of validation to perform.</param>
        /// <exception cref="System.ArgumentNullException">The value given was <see langword="null" />.</exception>
        [AssertionMethod]
        public static void Validate<T>([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [CanBeNull] [ValidatedNotNull] [NoEnumeration]
                                       this T value, [InvokerParameterName] [NotNull] string argumentName, ObjectIs argumentValidationType) where T : class
        {
            if(ReferenceEquals(value, null) && ((argumentValidationType & ObjectIs.NotNull) == ObjectIs.NotNull))
            {
                throw new ArgumentNullException(argumentName, ErrorMessages.ArgumentNull(argumentName));
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> meets the criteria specified in <paramref name="validationType" /> and
        /// throws an exception if it does not.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <exception cref="System.ArgumentNullException">The value given was <see langword="null" />.</exception>
        /// <exception cref="System.ArgumentException">The value given was zero-length.</exception>
        /// <exception cref="System.ArgumentException">The value given contains only white space characters.</exception>
        [AssertionMethod]
        public static void Validate([AssertionCondition(AssertionConditionType.IS_NOT_NULL)] [CanBeNull] [ValidatedNotNull]
                                    this string value, [InvokerParameterName] [NotNull] string argumentName, StringIs validationType)
        {
            if(ReferenceEquals(value, null))
            {
                if(validationType.HasFlag(StringIs.NotNull))
                {
                    throw new ArgumentNullException(argumentName, ErrorMessages.ArgumentNull(argumentName));
                }

                // It's null, so we can't validate the other conditions.
                return;
            }

            if(string.IsNullOrWhiteSpace(value))
            {
                if(value.Length <= 0)
                {
                    if(validationType.HasFlag(StringIs.NotEmpty))
                    {
                        throw new ArgumentException(ErrorMessages.ArgumentZeroLengthString(argumentName), argumentName);
                    }
                }
                else
                {
                    if(validationType.HasFlag(StringIs.NotWhiteSpace))
                    {
                        throw new ArgumentException(ErrorMessages.ArgumentWhiteSpaceString(argumentName), argumentName);
                    }
                }
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> is greater than, or less than, the value in <paramref name="limit" />,
        /// depending upon the setting in <paramref name="validationType" />.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="limit">
        /// The limit (maximum or minimum value depending upon the value in <paramref name="validationType" />
        /// ).
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">'value' is outside the permitted range of values.</exception>
        public static void Validate(this int value, [InvokerParameterName] [NotNull] string argumentName, NumberIs validationType, int limit)
        {
            switch(validationType)
            {
                case NumberIs.IsAtLeast:
                    if(value < limit)
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is less than the minimum value {1}.", argumentName, limit));
                    }

                    break;

                case NumberIs.IsAtMost:
                    if(value > limit)
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is more than the maximum value {1}.", argumentName, limit));
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException("validationType", validationType, "'validationType' must be 'NumberIs.IsAtLeast' or 'NumberIs.IsAtMost'.");
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> is in the inclusive range of values specified by
        /// <paramref name="lowerBoundInclusive" /> and <paramref name="upperBoundInclusive" /> and throws an exception if it is
        /// not.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="lowerBoundInclusive">The inclusive lower bound.</param>
        /// <param name="upperBoundInclusive">
        /// The inclusive upper bound (must not be less than
        /// <paramref name="lowerBoundInclusive" />)..
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">'value' is outside the permitted range of values.</exception>
        public static void Validate(this int value, [InvokerParameterName] [NotNull] string argumentName, NumberIs validationType, int lowerBoundInclusive, int upperBoundInclusive)
        {
            Debug.Assert(upperBoundInclusive >= lowerBoundInclusive, "Upper bound must not be less than lower bound.");

            if(((validationType == NumberIs.IsAtLeast) || (validationType == NumberIs.IsBetween)) && (value < lowerBoundInclusive))
            {
                throw new ArgumentOutOfRangeException(argumentName, value,
                                                      string.Format(CultureInfo.InvariantCulture, "'{0}' is less than the minimum value.  '{0}' must be in the range {1} to {2} (inclusive).",
                                                                    argumentName, lowerBoundInclusive, upperBoundInclusive));
            }

            if(((validationType == NumberIs.IsAtMost) || (validationType == NumberIs.IsBetween)) && (value > upperBoundInclusive))
            {
                throw new ArgumentOutOfRangeException(argumentName, value,
                                                      string.Format(CultureInfo.InvariantCulture, "'{0}' is greater than the maximum value.  '{0}' must be in the range {1} to {2} (inclusive).",
                                                                    argumentName, lowerBoundInclusive, upperBoundInclusive));
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> is greater than, or less than, the value in <paramref name="limit" />,
        /// depending upon the setting in <paramref name="validationType" />.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="limit">
        /// The limit (maximum or minimum value depending upon the value in <paramref name="validationType" />
        /// ).
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">'value' is outside the permitted range of values.</exception>
        public static void Validate(this long value, [InvokerParameterName] [NotNull] string argumentName, NumberIs validationType, long limit)
        {
            switch(validationType)
            {
                case NumberIs.IsAtLeast:
                    if(value < limit)
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is less than the minimum value {1}.", argumentName, limit));
                    }

                    break;

                case NumberIs.IsAtMost:
                    if(value > limit)
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is more than the maximum value {1}.", argumentName, limit));
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException("validationType", validationType, "'validationType' must be 'NumberIs.IsAtLeast' or 'NumberIs.IsAtMost'.");
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> is in the inclusive range of values specified by
        /// <paramref name="lowerBoundInclusive" /> and <paramref name="upperBoundInclusive" /> and throws an exception if it is
        /// not.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="lowerBoundInclusive">The inclusive lower bound.</param>
        /// <param name="upperBoundInclusive">
        /// The inclusive upper bound (must not be less than
        /// <paramref name="lowerBoundInclusive" />)..
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">'value' is outside the permitted range of values.</exception>
        public static void Validate(this long value, [InvokerParameterName] [NotNull] string argumentName, NumberIs validationType, long lowerBoundInclusive, long upperBoundInclusive)
        {
            Debug.Assert(upperBoundInclusive >= lowerBoundInclusive, "Upper bound must not be less than lower bound.");

            if(((validationType == NumberIs.IsAtLeast) || (validationType == NumberIs.IsBetween)) && (value < lowerBoundInclusive))
            {
                throw new ArgumentOutOfRangeException(argumentName, value,
                                                      string.Format(CultureInfo.InvariantCulture, "'{0}' is less than the minimum value.  '{0}' must be in the range {1} to {2} (inclusive).",
                                                                    argumentName, lowerBoundInclusive, upperBoundInclusive));
            }

            if(((validationType == NumberIs.IsAtMost) || (validationType == NumberIs.IsBetween)) && (value > upperBoundInclusive))
            {
                throw new ArgumentOutOfRangeException(argumentName, value,
                                                      string.Format(CultureInfo.InvariantCulture, "'{0}' is greater than the maximum value.  '{0}' must be in the range {1} to {2} (inclusive).",
                                                                    argumentName, lowerBoundInclusive, upperBoundInclusive));
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> is greater than, or less than, the value in <paramref name="limit" />,
        /// depending upon the setting in <paramref name="validationType" />.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="limit">
        /// The limit (maximum or minimum value depending upon the value in <paramref name="validationType" />
        /// ).
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">'value' is outside the permitted range of values.</exception>
        public static void Validate(this double value, [InvokerParameterName] [NotNull] string argumentName, NumberIs validationType, double limit)
        {
            switch(validationType)
            {
                case NumberIs.IsAtLeast:
                    if(value < limit)
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is less than the minimum value {1}.", argumentName, limit));
                    }

                    break;

                case NumberIs.IsAtMost:
                    if(value > limit)
                    {
                        throw new ArgumentOutOfRangeException(argumentName, value, string.Format(CultureInfo.InvariantCulture, "'{0}' is more than the maximum value {1}.", argumentName, limit));
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException("validationType", validationType, "'validationType' must be 'NumberIs.IsAtLeast' or 'NumberIs.IsAtMost'.");
            }
        }

        /// <summary>
        /// Asserts that <paramref name="value" /> is in the inclusive range of values specified by
        /// <paramref name="lowerBoundInclusive" /> and <paramref name="upperBoundInclusive" /> and throws an exception if it is
        /// not.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="argumentName">The name of the caller's argument passed in <paramref name="value" />.</param>
        /// <param name="validationType">The type of validation to perform.</param>
        /// <param name="lowerBoundInclusive">The inclusive lower bound.</param>
        /// <param name="upperBoundInclusive">
        /// The inclusive upper bound (must not be less than
        /// <paramref name="lowerBoundInclusive" />)..
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">'value' is outside the permitted range of values.</exception>
        public static void Validate(this double value, [InvokerParameterName] [NotNull] string argumentName, NumberIs validationType, double lowerBoundInclusive, double upperBoundInclusive)
        {
            Debug.Assert(upperBoundInclusive >= lowerBoundInclusive, "Upper bound must not be less than lower bound.");

            if(((validationType == NumberIs.IsAtLeast) || (validationType == NumberIs.IsBetween)) && (value < lowerBoundInclusive))
            {
                throw new ArgumentOutOfRangeException(argumentName, value,
                                                      string.Format(CultureInfo.InvariantCulture, "'{0}' is less than the minimum value.  '{0}' must be in the range {1} to {2} (inclusive).",
                                                                    argumentName, lowerBoundInclusive, upperBoundInclusive));
            }

            if(((validationType == NumberIs.IsAtMost) || (validationType == NumberIs.IsBetween)) && (value > upperBoundInclusive))
            {
                throw new ArgumentOutOfRangeException(argumentName, value,
                                                      string.Format(CultureInfo.InvariantCulture, "'{0}' is greater than the maximum value.  '{0}' must be in the range {1} to {2} (inclusive).",
                                                                    argumentName, lowerBoundInclusive, upperBoundInclusive));
            }
        }
    }
}