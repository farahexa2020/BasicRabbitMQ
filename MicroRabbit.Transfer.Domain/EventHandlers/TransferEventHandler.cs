using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interface;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static MicroRabbit.Domain.Core.Bus.IEventHandler;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {
        private readonly ITransferRepository _transferRepostory;
        public TransferEventHandler(ITransferRepository transferRepository)
        {
            this._transferRepostory = transferRepository;
        }
        public Task Handle(TransferCreatedEvent @event)
        {
            this._transferRepostory.Add(new TransferLog()
            {
                FromAccount = @event.From,
                ToAccount = @event.To,
                TransferAmount = @event.Amount
            });
            return Task.CompletedTask;
        }
    }
}
