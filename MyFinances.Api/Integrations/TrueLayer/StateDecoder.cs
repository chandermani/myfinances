using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinances.Api.Integrations
{
    public class StateDecoder : IStateDecoder
    {
        public State Decode(string state)
        {
            return new State(userEmail: state);
        }
    }
}
