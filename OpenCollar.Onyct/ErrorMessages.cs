// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>Functions that generate standard messages for exceptions.</summary>
    public static class ErrorMessages
    {
        /// <summary>The string used to indicate a <see langword="null" /> reference in error messages.</summary>
        [NotNull]
        public const string NullReference = "a [null] reference";

        /// <summary>Gets error message to use when a parameter is <see langword="null" />.</summary>
        /// <param name="parameterName">The name of the invalid parameter.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ArgumentNull([CanBeNull] string parameterName)
        {
            if(string.IsNullOrWhiteSpace(parameterName))
            {
                return @"Argument is null.";
            }

            return string.Format(CultureInfo.InvariantCulture, @"'{0}' is null.", parameterName);
        }

        /// <summary>Gets error message to use when a string parameter is zero-length.</summary>
        /// <param name="parameterName">The name of the invalid parameter.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ArgumentZeroLengthString([CanBeNull] string parameterName)
        {
            if(string.IsNullOrWhiteSpace(parameterName))
            {
                return @"Argument is zero-length.";
            }

            return string.Format(CultureInfo.InvariantCulture, @"'{0}' is zero-length.", parameterName);
        }

        /// <summary>Gets error message to use when a string parameter contains only white space characters.</summary>
        /// <param name="parameterName">The name of the invalid parameter.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ArgumentWhiteSpaceString([CanBeNull] string parameterName)
        {
            if(string.IsNullOrWhiteSpace(parameterName))
            {
                return @"Argument contains only white space characters.";
            }

            return string.Format(CultureInfo.InvariantCulture, @"'{0}' contains only white space characters.", parameterName);
        }

        /// <summary>
        /// Gets error message to use when a method or property of a <see cref="IDisposable">Disposable</see> object is
        /// called after it has been disposed of.
        /// </summary>
        /// <param name="disposableObjectType">The type of the object that has been disposed of.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ObjectAccessedAfterDispose([CanBeNull] Type disposableObjectType)
        {
            return string.Format(CultureInfo.InvariantCulture, "Object of type {0} cannot be accessed after it has been disposed of.", GetRepresentation(disposableObjectType));
        }

        /// <summary>
        /// Gets error message to use when the implementor of an abstract or virtual method in a base class returns a
        /// value that is not permitted.
        /// </summary>
        /// <param name="baseClassType">The type of the base class.</param>
        /// <param name="derivedClassType">The type of the derived class.</param>
        /// <param name="member">The member (on the derived class) that returned an invalid value.</param>
        /// <param name="invalidValueDescription">
        /// A description of the invalid value (must fit into "The derived class returned XXX
        /// when the method was called.
        /// </param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ImplementorReturnedInvalid([CanBeNull] Type baseClassType, [CanBeNull] Type derivedClassType, [CanBeNull] MemberInfo member, [CanBeNull] string invalidValueDescription)
        {
            return string.Format(CultureInfo.InvariantCulture, "The derived class ({0}, derived from {1}) returned {2} when the {3} method was called.", GetRepresentation(derivedClassType),
                GetRepresentation(baseClassType), invalidValueDescription ?? NullReference, GetRepresentation(member));
        }

        /// <summary>
        /// Gets error message to use when the implementor of an abstract or virtual method in a base class returns null
        /// when it is not permitted.
        /// </summary>
        /// <param name="baseClassType">The type of the base class.</param>
        /// <param name="derivedClassType">The type of the derived class.</param>
        /// <param name="member">The member (on the derived class) that returned <see langword="null" />.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ImplementorReturnedNull([CanBeNull] Type baseClassType, [CanBeNull] Type derivedClassType, [CanBeNull] MemberInfo member)
        {
            return ImplementorReturnedInvalid(baseClassType, derivedClassType, member, NullReference);
        }

        /// <summary>Gets a string that can be used to represent the type given in error messages.</summary>
        /// <param name="type">The type to represent.</param>
        /// <returns>A string that can be used to represent the type given in error messages.</returns>
        [NotNull]
        public static string GetRepresentation([CanBeNull] Type type)
        {
            if(ReferenceEquals(type, null))
            {
                return "[unknown type]";
            }

            return "[" + type.FullName + "]";
        }

        /// <summary>Gets a string that can be used to represent the member given in error messages.</summary>
        /// <param name="member">The member to represent.</param>
        /// <returns>A string that can be used to represent the member given in error messages.</returns>
        [NotNull]
        public static string GetRepresentation([CanBeNull] MemberInfo member)
        {
            if(ReferenceEquals(member, null))
            {
                return "[unknown member]";
            }

            var method = member as MethodInfo;
            if(!ReferenceEquals(method, null))
            {
                return "'" + method.Name + "(" + string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name)) + ")'";
            }

            var property = member as PropertyInfo;
            if(!ReferenceEquals(property, null))
            {
                return "'" + property.Name + "'";
            }

            var eventInfo = member as EventInfo;
            if(!ReferenceEquals(eventInfo, null))
            {
                return "'" + eventInfo.Name + "'";
            }

            return "'" + member.Name + "'";
        }
    }
}