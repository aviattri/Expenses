
using System;

namespace Expenses.Core.DTO
{
    public class Expense
    {    
         public int ID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public static explicit operator Expense(DB.Expense e) => new Expense
        {
             ID = e.ID,
             Amount = e.Amount,
             Description = e.Description
         };
    }
}
