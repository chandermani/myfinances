using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Integrations.TrueLayer
{

    [Serializable]
    public class TrueLayerIntegrationException : Exception
    {
        public TrueLayerIntegrationException() { }
        public TrueLayerIntegrationException(string message) : base(message) { }
        public TrueLayerIntegrationException(string message, Exception inner) : base(message, inner) { }
        protected TrueLayerIntegrationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class TokenNotFoundException : TrueLayerIntegrationException
    {
        public TokenNotFoundException() { }
        public TokenNotFoundException(string message) : base(message) { }
        public TokenNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected TokenNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
