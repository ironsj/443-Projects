using BankingSystem.Data;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Services
{
    public class BankService : IBankService
    {
        private readonly BankingSystemContext _context;

        public BankService(BankingSystemContext context)
        {
            _context = context;
        }

        public void Update(BankingSystem.Models.Transaction.Actions action, Account account, BankingSystem.Models.Transaction transaction)
        {
            // Updates the transaction nullable properties
            transaction.NewBalance = account.Balance;
            transaction.TimeSlot = System.DateTime.Now;
            transaction.Action = action;  // Transaction.Actions.deposit;
        }

    }
}
