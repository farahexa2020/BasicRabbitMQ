using MicroRabbit.transfer.Application.Intefaces;
using MicroRabbit.Transfer.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.Interface;

namespace MicroRabbit.transfer.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IEventBus _bus;

        public TransferService(ITransferRepository transferRepository, IEventBus bus)
        {
            this._transferRepository = transferRepository;
            this._bus = bus;
        }

        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return this._transferRepository.GetTransferLogss();
        }
    }
}
