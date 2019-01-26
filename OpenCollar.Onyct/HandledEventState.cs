namespace OpenCollar.Onyct
{
    /// <summary>The state of an event that can be handled whilst it is being raised.</summary>
    public enum HandledEventState
    {
        /// <summary>
        ///     The event has not been handled and will continue to be raised to consumers.
        /// </summary>
        Unhandled = 0,

        /// <summary>
        ///     The event has been handled but can continue to be raised to consumers.
        /// </summary>
        HandledButContinueToNotify,

        /// <summary>
        ///     The event has been handled and no further consumers should be notified.
        /// </summary>
        HandledAndCeaseRaising
    }
}