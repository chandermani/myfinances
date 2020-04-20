using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core
{
    public interface IClock
    {
        DateTime Now { get; }
    }

    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now;
    }
}
