using MicroRabbit.Banking.Application.Intefaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interface;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEventBus _bus;

        public AccountService(IAccountRepository accountRepository, IEventBus bus)
        {
            this._accountRepository = accountRepository;
            this._bus = bus;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return this._accountRepository.GetAccounts();
        }

        public void Transfer(AccountTransfer accountTransfer)
        {
            var TransferCommand = new CreateTransferCommand(
                    accountTransfer.FromAccount,
                    accountTransfer.ToAccount,
                    accountTransfer.TransferAmount
                );

            this._bus.SendCommand(TransferCommand);
        }
    }
}
