// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>
    /// An exception that occurs when a derived class does not implement an <c>abstract</c> or <c>virtual</c> method
    /// correctly.
    /// </summary>
    [Serializable]
    public class BadImplementationException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="BadImplementationException" /> class.</summary>
        public BadImplementationException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadImplementationException" /> class with a specified error
        /// message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public BadImplementationException([CanBeNull] string message) : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadImplementationException" /> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a <see langword="null" /> if
        /// no inner exception is specified.
        /// </param>
        public BadImplementationException([CanBeNull] string message, [CanBeNull] Exception innerException) : base(message, innerException)
        { }

        /// <summary>Initializes a new instance of the <see cref="BadImplementationException" /> class with serialized data.</summary>
        /// <param name="info">
        /// The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object
        /// data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual
        /// information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />. </exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        /// The class name is <see langword="null"/> or
        /// <see cref="System.Exception.HResult" /> is zero (0).
        /// </exception>
        protected BadImplementationException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}