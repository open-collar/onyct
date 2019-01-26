// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>The base-class for event arguments that can control the behavior of the code that invokes each callback handler.</summary>
    /// <remarks>Use the <see cref="HandledEventArgs.State"/> property to control how the event is raised to subsequent handlers.</remarks>
    /// <seealso cref="System.EventArgs" />
    public abstract class HandledEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets or sets the current state of this event.
        /// </summary>
        /// <value>
        ///     The current state of this event.
        /// </value>
        public HandledEventState State { get; set; } = HandledEventState.Unhandled;
    }
}