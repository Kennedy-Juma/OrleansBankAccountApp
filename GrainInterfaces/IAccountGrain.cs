using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.BankAccountApp.Models;

namespace Orleans.BankAccountApp.GrainInterfaces
{
    /// <summary>
    /// Orleans grain interface for a bank account.
    /// Uses a GUID as the grain key (account ID).
    /// </summary>
    public interface IAccountGrain:IGrainWithGuidKey
    {
        Task Deposit(decimal amount);
        Task Withdraw(decimal amount);
        Task<decimal> GetBalance();
        Task<List<TransactionRecord>> GetTransactionHistory();
    }
}
