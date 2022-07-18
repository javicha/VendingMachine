namespace Wallet.Domain.Exceptions
{
    [Serializable]
    public class WrongCoinAmountException : Exception
    {
        public WrongCoinAmountException() { }
        public WrongCoinAmountException(string message) : base(message) { }
        public WrongCoinAmountException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected WrongCoinAmountException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
