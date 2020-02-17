using MicroRabbit.Banking.Domain.Interface;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interface;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _context;

        public TransferRepository(TransferDbContext context)
        {
            this._context = context;
        }

        public IEnumerable<TransferLog> GetTransferLogss()
        {
            return this._context.TransferLog;
        }

        public void Add(TransferLog transferLog)
        {
            this._context.Add(transferLog);
            this._context.SaveChanges();
        }
    }
}
