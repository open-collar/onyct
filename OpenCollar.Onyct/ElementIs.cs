// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;

namespace OpenCollar.Onyct
{
    /// <summary>The basic content validation for element in enumerables.</summary>
    [Flags]
    public enum ElementIs
    {
        /// <summary>Validation type has not been specified.</summary>
        Unknown = 0,

        /// <summary>The element may not be <see langword="null" />.</summary>
        NotNull = 1
    }
}