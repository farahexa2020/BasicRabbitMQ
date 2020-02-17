using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Domain.Interface
{
    public interface ITransferRepository
    {
        IEnumerable<TransferLog> GetTransferLogss();
        void Add(TransferLog transfer);
    }
}
