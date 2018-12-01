// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>The arguments supplied to handlers of the <see cref="ExceptionManager.UnhandledException" /> event.</summary>
    /// <seealso cref="System.EventArgs" />
    public sealed class UnhandledExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// A short description of the activity that was taking place at the time of the exception (should be a sentence
        /// fragment with no initial capital or terminal punctuation,  e.g. "loading tasks").  Can be <see langword="null" />,
        /// zero-length or contain only whitespace characters.
        /// </summary>
        [CanBeNull]
        private readonly string _activity;

        /// <summary>The unhandled exception that has been detected.  Can be <see langword="null" />.</summary>
        [CanBeNull]
        private readonly Exception _exception;

        /// <summary>Initializes a new instance of the <see cref="UnhandledExceptionEventArgs" /> class.</summary>
        /// <param name="exception">The unhandled exception that has been detected.  Can be <see langword="null" />.</param>
        /// <param name="activity">
        /// A short description of the activity taking place when the unhandled exception occurred.  Can be
        /// <see langword="null" />, zero-length or contain only whitespace characters.
        /// </param>
        public UnhandledExceptionEventArgs([CanBeNull] Exception exception, [CanBeNull] string activity)
        {
            _exception = exception;
            _activity = activity;
        }

        /// <summary>Gets the unhandled exception that has been detected.  Can be <see langword="null" />.</summary>
        /// <value>The unhandled exception that has been detected.  Can be <see langword="null" />.</value>
        [CanBeNull]
        public Exception Exception
        {
            get { return _exception; }
        }

        /// <summary>
        /// Gets a short description of the activity that was taking place at the time of the exception (should be a
        /// sentence fragment with no initial capital or terminal punctuation,  e.g. "loading tasks").  Can be
        /// <see langword="null" />, zero-length or contain only whitespace characters.
        /// </summary>
        /// <value>
        /// A short description of the activity that was taking place at the time of the exception (should be a sentence
        /// fragment with no initial capital or terminal punctuation,  e.g. "loading tasks").  Can be <see langword="null" />,
        /// zero-length or contain only whitespace characters.
        /// </value>
        [CanBeNull]
        public string Activity
        {
            get { return _activity; }
        }
    }
}