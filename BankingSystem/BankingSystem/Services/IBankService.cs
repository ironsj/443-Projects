using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace BankingSystem.Services
{
    public interface IBankService
    {

        public void Update(BankingSystem.Models.Transaction.Actions action, Account account, BankingSystem.Models.Transaction transaction);
    }
}
