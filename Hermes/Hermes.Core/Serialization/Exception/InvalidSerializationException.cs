using System;

namespace Hermes.Core.Serialization
{
    /// <summary>
	/// Exception for invalid serializations.
	/// </summary>
    public sealed class InvalidSerializationException : Exception
    {
        /// <summary>
        /// Default construction.
        /// </summary>
        public InvalidSerializationException() { }
        /// <summary>
        /// Construction specifying it's description or help message.
        /// </summary>
        /// <param name="message">Exception description.</param>
        public InvalidSerializationException(string message) 
            : base(message) { }
        /// <summary>
        /// Construction specifying it's message and inner exception, if any.
        /// </summary>
        /// <param name="message">Exception description.</param>
        /// <param name="inner">Inner exception.</param>
        public InvalidSerializationException(string message, Exception inner) 
            : base(message, inner) { }
    }
}
