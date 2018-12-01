// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

namespace OpenCollar.Onyct
{
    /// <summary>Defines the ways in which an event args factory should be used.</summary>
    public enum ArgsUsage
    {
        /// <summary>Usage is undefined (this will cause an error if used).</summary>
        Unknown = 0,

        /// <summary>The factory will be called once and the same instance of the event args will be used for all delegates.</summary>
        Reuse,

        /// <summary>The factory will be called separately for each delegate.</summary>
        UniqueInstance
    }
}