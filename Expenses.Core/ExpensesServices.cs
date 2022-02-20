using Expenses.DB;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Core
{
    public class ExpensesServices : IExpensesServices
    {    //constructor takes an instance of app db context
        private AppDbContext _context;
        public ExpensesServices(AppDbContext context)
        {  //dependency injection
            _context = context;
        }
        public Expense GetExpense(int id)
        {   //return the first expense that matches the ID in the expenses list
            return _context.Expenses.First(e => e.ID == id);
        }

        public Expense CreateExpense(Expense expense)
        {   //add the exepnse to the DB
            _context.Add(expense);
            //save the changes and return expense
            _context.SaveChanges();
            return expense;
        }

        public void DeleteExpense(Expense expense)
        {
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
        }
        public Expense EditExpense(Expense expense)
        {
            var dbExpense = _context.Expenses.First(e => e.ID == expense.ID);
            dbExpense.Description = expense.Description;
            dbExpense.Amount = expense.Amount;

            _context.SaveChanges();

            return dbExpense;
        }

        public List<Expense> GetExpenses()
        {   //use instance of context to access Expenses and return everything as a list  
            return _context.Expenses.ToList();
        }
    }

}
