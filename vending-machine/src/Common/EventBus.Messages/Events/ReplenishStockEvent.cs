namespace EventBus.Messages.Events
{
    /// <summary>
    /// Event that notificates to replenish the product stock
    /// </summary>
    public class ReplenishStockEvent
    {
        /// <summary>
        /// Product identifier
        /// </summary>
        public int ProductId { get; set; }

        public ReplenishStockEvent(int productId)
        {
            ProductId = productId;
        }
    }
}
