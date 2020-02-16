using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Domain.Core.Events
{
    public class Event
    {
        public DateTime TimeStamp { get; protected set; }

        protected Event()
        {
            this.TimeStamp = DateTime.Now;
        }
    }
}
