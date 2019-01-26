// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace OpenCollar.Onyct
{
    /// <summary>A base class for disposable objects.</summary>
    public abstract class Disposable : IDisposable
    {
        /// <summary>The value of the <see cref="_isDisposed" /> flag before the class has started to be disposed of.</summary>
        private const int NotDisposed = default(int);

        /// <summary>The value of the <see cref="_isDisposed" /> flag after the class has started to be disposed of.</summary>
        private const int Disposed = NotDisposed + 1;

        /// <summary>A flag used to track whether this instance has been disposed of.</summary>
        private int _isDisposed;/* = NotDisposed; // This will be automatically instantiated correctly, no need for us to do anything else. */

        /// <summary>Gets a value indicating whether this instance has been disposed of.</summary>
        /// <value><see langword="true" /> if this instances has been disposed of; otherwise, <see langword="false" />.</value>
        public bool IsDisposed => _isDisposed == Disposed;

        /// <summary>Release allocated resources.</summary>
        public void Dispose()
        {
            // Check to see if we are already disposing.
            if(Interlocked.CompareExchange(ref _isDisposed, Disposed, NotDisposed) == Disposed)
            {
                return;
            }

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing">
        /// <see langword="true" /> to release both managed and unmanaged resources;
        /// <see langword="false" /> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // In many use cases this will not be used or required - so a default implementation is provided.
        }

        /// <summary>Checks that this object has not been disposed of and throws an exception if it has.</summary>
        /// <exception cref="ObjectDisposedException">Object cannot be accessed after it has been disposed of.</exception>
        protected void CheckNotDisposed()
        {
            if(_isDisposed != NotDisposed)
            {
                throw new ObjectDisposedException(ErrorMessages.ObjectAccessedAfterDispose(GetType()));
            }
        }
    }
}