namespace Domain.Exceptions
{
    [Serializable]
    public class WrongCurrencyException : Exception
    {
        public WrongCurrencyException() { }
        public WrongCurrencyException(string message) : base(message) { }
        public WrongCurrencyException(string message, Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected WrongCurrencyException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
}
