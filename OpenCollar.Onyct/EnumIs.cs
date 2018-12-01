// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;

namespace OpenCollar.Onyct
{
    /// <summary>The basic content validation for objects.</summary>
    [Flags]
    public enum EnumIs
    {
        /// <summary>ValidationExtensions type has not been specified.</summary>
        Unknown = 0,

        /// <summary>The value is a valid member of the enum.</summary>
        Member = 1,

        /// <summary>The value is a valid member of the enum and is not zero (usually reserved from 'Unknown').</summary>
        NonZeroMember = 2
    }
}