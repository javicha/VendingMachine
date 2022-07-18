namespace Wallet.Domain.Common
{
    /// <summary>
    /// Class with the fields common to all domain entities (for example unique identifier or audit data)
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; protected set; } //Protected set in order to use in derived classes
        public DateTime DateCreated { get; set; }
    }
}
