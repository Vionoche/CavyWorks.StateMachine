using System;
using System.Runtime.Serialization;

namespace CavyWorks.StateMachine
{
    public class TransitException: Exception
    {
        public TransitException()
        {
        }

        protected TransitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TransitException(string message) : base(message)
        {
        }

        public TransitException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}