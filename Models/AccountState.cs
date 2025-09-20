using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.BankAccountApp.Models
{
    /// <summary>
    /// Internal state of the account grain (balance and transaction list).
    /// </summary>
    [Serializable]
    public class AccountState
    {
        public decimal Balance { get; set; } = 0;
        public List<TransactionRecord> Transactions { get; set; } = new List<TransactionRecord>();
    }
}
