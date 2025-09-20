using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.BankAccountApp.GrainInterfaces;
using Orleans.BankAccountApp.Models;
using Orleans.Runtime;

namespace Orleans.BankAccountApp.Grains
{
    /// <summary>
    /// Grain implementation for a bank account.
    /// Uses in-memory persistent state for balance and history.
    /// </summary>
    public class AccountGrain:Grain,IAccountGrain
    {
        private readonly IPersistentState<AccountState> _accountState;
        public AccountGrain(
            [PersistentState("accountState", "MemoryStore")]
            IPersistentState<AccountState> accountState)
        {
            _accountState = accountState;
        }

        public async Task Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.");
            _accountState.State.Balance += amount;
            _accountState.State.Transactions.Add(new TransactionRecord
            {
                Timestamp = DateTime.UtcNow,
                Type = "Deposit",
                Amount = amount
            });
            await _accountState.WriteStateAsync();
        }

        public async Task Withdraw(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.");
            if (_accountState.State.Balance < amount)
            {
                throw new InvalidOperationException($"Insufficient funds: current balance is {_accountState.State.Balance}.");
            }
            _accountState.State.Balance -= amount;
            _accountState.State.Transactions.Add(new TransactionRecord
            {
                Timestamp = DateTime.UtcNow,
                Type = "Withdraw",
                Amount = amount
            });
            await _accountState.WriteStateAsync();
        }

        public Task<decimal> GetBalance() => Task.FromResult(_accountState.State.Balance);

        public Task<List<TransactionRecord>> GetTransactionHistory()
            => Task.FromResult(new List<TransactionRecord>(_accountState.State.Transactions));

    }
}
