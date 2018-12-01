// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;

namespace OpenCollar.Onyct
{
    /// <summary>The basic content validation for objects.</summary>
    [Flags]
    public enum NumberIs
    {
        /// <summary>ValidationExtensions type has not been specified.</summary>
        Unknown = 0,

        /// <summary>The number is greater than or equal to the number specified.</summary>
        IsAtLeast,

        /// <summary>The value is less than or equal to the number specified.</summary>
        IsAtMost,

        /// <summary>
        /// The value greater than or equal to the first value specified, and less than or equal to the second number
        /// specified.
        /// </summary>
        IsBetween
    }
}