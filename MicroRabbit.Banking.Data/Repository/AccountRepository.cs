using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interface;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _context;

        public AccountRepository(BankingDbContext context)
        {
            this._context = context;
        }
        public IEnumerable<Account> GetAccounts()
        {
            return this._context.Accounts;
        }
    }
}
