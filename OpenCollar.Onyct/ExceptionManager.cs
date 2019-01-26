// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>A hub through which all unhandled exceptions are routed.</summary>
    public static class ExceptionManager
    {
        /// <summary>Called when an unhandled exception is reported.</summary>
        public static event EventHandler<UnhandledExceptionEventArgs> UnhandledException;

        /// <summary>Called when unhandled exception occurs anywhere in an application or its libraries.</summary>
        /// <param name="unhandledException">The exception that was thrown.  Can be <see langword="null" />.</param>
        /// <param name="activity">
        /// The activity that was taking place at the time of the exception (should be a sentence fragment
        /// with no initial capital or terminal punctuation,  e.g. "loading tasks").  Can be <see langword="null" />, zero-length
        /// or contain only whitespace characters.
        /// </param>
        public static void OnUnhandledException([CanBeNull] Exception unhandledException, [CanBeNull] string activity)
        {
            UnhandledException.SafeInvoke(nameof(UnhandledException), null, ArgsUsage.Reuse, () => new UnhandledExceptionEventArgs(unhandledException, activity));
        }
    }
}