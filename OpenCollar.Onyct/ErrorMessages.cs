// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>Functions that generate standard messages for exceptions.</summary>
    public static class ErrorMessages
    {
        /// <summary>The string used to indicate a <see langword="null" /> reference in error messages.</summary>
        [NotNull] public const string NullReference = @"a [null] reference";

        /// <summary>The string used to indicate a zero-length string in error messages.</summary>
        [NotNull] public const string ZeroLength = @"a zero-length string";

        /// <summary>The string used to indicate a white-space-only string in error messages.</summary>
        [NotNull] public const string WhiteSpaceOnly = @"a string containing only white space characters";

        /// <summary>Gets error message to use when a parameter is <see langword="null" />.</summary>
        /// <param name="parameterName">The name of the invalid parameter.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ArgumentNull([CanBeNull] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) return $@"Argument is {NullReference}.";

            return $@"'{parameterName}' is {NullReference}.";
        }

        /// <summary>Gets error message to use when a string parameter is zero-length.</summary>
        /// <param name="parameterName">The name of the invalid parameter.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ArgumentZeroLengthString([CanBeNull] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) return $@"Argument is {ZeroLength}.";

            return $@"'{parameterName}' is {ZeroLength}.";
        }

        /// <summary>Gets error message to use when a string parameter contains only white space characters.</summary>
        /// <param name="parameterName">The name of the invalid parameter.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ArgumentWhiteSpaceString([CanBeNull] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName)) return $@"Argument is {WhiteSpaceOnly}.";

            return $@"'{parameterName}' is {WhiteSpaceOnly}.";
        }

        /// <summary>
        ///     Gets error message to use when a method or property of a <see cref="IDisposable">Disposable</see> object is
        ///     called after it has been disposed of.
        /// </summary>
        /// <param name="disposableObjectType">The type of the object that has been disposed of.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ObjectAccessedAfterDispose([CanBeNull] Type disposableObjectType)
        {
            return
                $@"Object of type {GetRepresentation(disposableObjectType)} cannot be accessed after it has been disposed of.";
        }

        /// <summary>
        ///     Gets error message to use when the implementor of an abstract or virtual method in a base class returns a
        ///     value that is not permitted.
        /// </summary>
        /// <param name="baseClassType">The type of the base class.</param>
        /// <param name="derivedClassType">The type of the derived class.</param>
        /// <param name="member">The member (on the derived class) that returned an invalid value.</param>
        /// <param name="invalidValueDescription">
        ///     A description of the invalid value (must fit into "The derived class returned XXX
        ///     when the method was called.
        /// </param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ImplementorReturnedInvalid([CanBeNull] Type baseClassType,
            [CanBeNull] Type derivedClassType, [CanBeNull] MemberInfo member,
            [CanBeNull] string invalidValueDescription)
        {
            return
                $@"The derived class ({GetRepresentation(derivedClassType)}, derived from {GetRepresentation(baseClassType)}) returned {invalidValueDescription ?? NullReference} when the {GetRepresentation(member)} method was called.";
        }

        /// <summary>
        ///     Gets error message to use when the implementor of an abstract or virtual method in a base class returns null
        ///     when it is not permitted.
        /// </summary>
        /// <param name="baseClassType">The type of the base class.</param>
        /// <param name="derivedClassType">The type of the derived class.</param>
        /// <param name="member">The member (on the derived class) that returned <see langword="null" />.</param>
        /// <returns>A formatted string ready to use in the error message.</returns>
        [NotNull]
        public static string ImplementorReturnedNull([CanBeNull] Type baseClassType, [CanBeNull] Type derivedClassType,
            [CanBeNull] MemberInfo member)
        {
            return ImplementorReturnedInvalid(baseClassType, derivedClassType, member, NullReference);
        }

        /// <summary>Gets a string that can be used to represent the type given in error messages.</summary>
        /// <param name="type">The type to represent.</param>
        /// <returns>A string that can be used to represent the type given in error messages.</returns>
        [NotNull]
        public static string GetRepresentation([CanBeNull] Type type)
        {
            if (ReferenceEquals(type, null)) return @"[unknown type]";

            return string.Concat(@"[", type.FullName, @"]");
        }

        /// <summary>Gets a string that can be used to represent the member given in error messages.</summary>
        /// <param name="member">The member to represent.</param>
        /// <returns>A string that can be used to represent the member given in error messages.</returns>
        [NotNull]
        public static string GetRepresentation([CanBeNull] MemberInfo member)
        {
            if (ReferenceEquals(member, null)) return @"[unknown member]";

            var method = member as MethodInfo;
            if (!ReferenceEquals(method, null))
                return string.Concat(@"'", method.Name, @"(",
                    string.Join(@", ", method.GetParameters().Select(p => p.ParameterType.Name)), @")'");

            var property = member as PropertyInfo;
            if (!ReferenceEquals(property, null)) return string.Concat(@"'", property.Name, @"'");

            var eventInfo = member as EventInfo;
            if (!ReferenceEquals(eventInfo, null)) return string.Concat(@"'", eventInfo.Name, @"'");

            return string.Concat(@"'", member.Name, @"'");
        }
    }
}