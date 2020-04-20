using System;
using System.Collections.Generic;
using System.Text;

namespace MyFinances.Core.Model
{
    public class Transaction
    {
        public Transaction(string identifier, DateTime time, string category, decimal amount, string currencyCode)
        {
            Identifier = identifier;
            Time = time;
            Category = category;
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public string Identifier { get; }
        public DateTime Time { get; }
        public string Category { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
    }
}
