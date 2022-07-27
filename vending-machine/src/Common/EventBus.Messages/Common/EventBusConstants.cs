namespace EventBus.Messages.Common
{
    /// <summary>
    /// Constants with the correspondence between the events and the queue in which they are published/consumed
    /// </summary>
    public static class EventBusConstants
    {
        public static readonly string CalculateDifferenceQueue = "CalculateDifference.Queue";
        public static readonly string UpdateCoinStockQueue = "UpdateCoinStock.Queue";
    }
}
