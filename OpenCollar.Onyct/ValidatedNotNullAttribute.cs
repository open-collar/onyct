// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Diagnostics.CodeAnalysis;

namespace OpenCollar.Onyct
{
    /// <summary>Class used to decorate the arguments of a function that are validated as not null by a validation method.</summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    [ExcludeFromCodeCoverage]
    public sealed class ValidatedNotNullAttribute : Attribute
    { }
}