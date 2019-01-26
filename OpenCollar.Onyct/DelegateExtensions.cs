// Copyright © 2018 Jonathan David Evans
// All Rights Reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace OpenCollar.Onyct
{
    /// <summary>A delegate used to generate event args for a safe call to a delegate.</summary>
    /// <typeparam name="TEventArgs">The type of the event args that will be returned.</typeparam>
    /// <returns>A instance of the event args to pass to the delegate when invoked.</returns>
    public delegate TEventArgs EventArgsFactory<out TEventArgs>() where TEventArgs : EventArgs;

    /// <summary>Extensions to the delegate class.</summary>
    public static class DelegateExtensions
    {
        /// <summary>Gets the delegate description.</summary>
        /// <param name="delegate">The delegate to describe.</param>
        /// <returns>A description of the type and method specified by the delegate.</returns>
        [NotNull]
        public static string GetDescription([CanBeNull] this Delegate @delegate)
        {
            if(ReferenceEquals(@delegate, null))
            {
                return @"[null]";
            }

            if(ReferenceEquals(@delegate.Method.DeclaringType, null))
            {
                return $@"[unknown].{@delegate.Method}";
            }

            // ReSharper disable once AssignNullToNotNullAttribute
            return $@"[{@delegate.Method.DeclaringType.FullName}].{@delegate.Method}";
        }

        /// <summary>
        /// Invokes the delegate given (if not <see langword="null" />) with protection against exceptions thrown by the
        /// invoked methods.
        /// </summary>
        /// <param name="handler">The delegate to call.</param>
        /// <param name="eventName">The name of the event being raised.</param>
        /// <param name="sender">The object to pass as the sender.</param>
        /// <returns><see langword="true" /> if at least one delegate was successfully invoked, <see langword="false" /> otherwise.</returns>
        public static bool SafeInvoke([CanBeNull] this Delegate handler, [NotNull] string eventName, [CanBeNull] object sender)
        {
            return SafeInvoke(handler, eventName, sender, ArgsUsage.Reuse, () => EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the delegate given (if not <see langword="null" />) with protection against exceptions thrown by the
        /// invoked methods.
        /// </summary>
        /// <typeparam name="T">The type of the event args to pass.</typeparam>
        /// <param name="handler">The delegate to call.</param>
        /// <param name="eventName">The name of the event being raised.</param>
        /// <param name="sender">The object to pass as the sender.</param>
        /// <param name="usage">The way in which to use the factory to generate args.</param>
        /// <param name="eventArgFactory">A factory for generating event args.</param>
        /// <returns><see langword="true" /> if at least one delegate was successfully invoked, <see langword="false" /> otherwise.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if an invalid value passed in the <paramref name="usage" />
        /// argument.
        /// </exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="eventName" /> is <see langword="null" />.</exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="eventName" /> is zero-length or contains only white space
        /// characters.
        /// </exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="eventArgFactory" /> is <see langword="null" />.</exception>
        public static bool SafeInvoke<T>([CanBeNull] this Delegate handler, [NotNull] string eventName, [CanBeNull] object sender, ArgsUsage usage, EventArgsFactory<T> eventArgFactory)
            where T : EventArgs
        {
            eventName.Validate(nameof(eventName), StringIs.NotNullEmptyOrWhiteSpace);
            eventArgFactory.Validate(nameof(eventArgFactory), ObjectIs.NotNull);

            // An empty invocation list is null.
            if(ReferenceEquals(handler, null))
            {
                return false;
            }

            var delegates = handler.GetInvocationList();
            // All non-null invocation lists are also non-empty, so no need to test here.

            object[] args = null;
            bool generateArgs;
            switch(usage)
            {
                case ArgsUsage.Reuse:
                    args = new[] {sender, eventArgFactory()};
                    generateArgs = false;
                    break;

                case ArgsUsage.UniqueInstance:
                    generateArgs = true;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(usage), usage, $@"'{nameof(usage)}' argument contains invalid value: {usage}.");
            }

            var raised = false;
            var checkForHandled = typeof(HandledEventArgs).IsAssignableFrom(typeof(T));

            foreach (var callback in delegates)
            {
                try
                {
                    if (generateArgs)
                    {
                        args = new[] {sender, eventArgFactory()};
                    }

                    callback.DynamicInvoke(args);
                    raised = true;

                    if (checkForHandled && (((HandledEventArgs) args[1]).State == HandledEventState.HandledAndCeaseRaising))
                    {
                        break;
                    }
                }
                catch (ThreadAbortException)
                {
                    // Ignore thread abort exceptions, but do stop processing any further.
                    break;
                }
                catch(Exception ex)
                {
                    if (typeof(UnhandledExceptionEventArgs).IsAssignableFrom(typeof(T)))
                    {
                        // If we already in the process of raising an exception just ignore anything further.
                        continue;
                    }

                    // If an event handler has raised an exception report it, but carry on.
                    ExceptionManager.OnUnhandledException(ex, $@"Error whilst '{eventName}' event was being handled by {GetDescription(callback)}.");
                }
            }

            return raised;
        }
    }
}