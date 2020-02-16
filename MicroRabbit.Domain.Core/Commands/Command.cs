using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Domain.Core.Commands
{
    public class Command : Message
    {
        public DateTime TimeStamp { get; protected set; }

        protected Command()
        {
            this.TimeStamp = DateTime.Now;
        }
    }
}
