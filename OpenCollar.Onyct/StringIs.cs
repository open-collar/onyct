// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;

namespace OpenCollar.Onyct
{
    /// <summary>The basic content validation for strings.</summary>
    [Flags]
    public enum StringIs
    {
        /// <summary>Validation type has not been specified.</summary>
        Unknown = 0,

        /// <summary>The string may not be <see langword="null" />.</summary>
        NotNull = 1,

        /// <summary>The string may not be zero-length.</summary>
        NotEmpty = 2,

        /// <summary>The string may not contain only white space characters.</summary>
        NotWhiteSpace = 4,

        /// <summary>The string may not be <see langword="null" />, zero-length or contain only white space characters.</summary>
        NotNullEmptyOrWhiteSpace = NotNull | NotEmpty | NotWhiteSpace,

        /// <summary>The string may not be <see langword="null" /> or zero-length.</summary>
        NotNullOrEmpty = NotNull | NotEmpty,

        /// <summary>The string may not be <see langword="null" /> or contain only white space characters.</summary>
        NotNullOrWhiteSpace = NotNull | NotWhiteSpace,

        /// <summary>The string may not be zero-length or contain only white space characters.</summary>
        NotEmptyOrWhiteSpace = NotEmpty | NotWhiteSpace
    }
}