using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.BankAccountApp.Models
{
    /// <summary>
    /// A record of a single transaction (deposit or withdrawal).
    /// </summary>
    [GenerateSerializer]
    [Immutable]
    public class TransactionRecord
    {
        [Id(0)]
        public DateTime Timestamp { get; set; }
        [Id(1)]
        public string Type { get; set; }
        [Id(2)]
        public decimal Amount { get; set; }
    }
}
