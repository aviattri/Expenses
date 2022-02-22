using Expenses.Core.DTO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Core
{
    public class ExpensesServices : IExpensesServices
    {    //constructor takes an instance of app db context
        private DB.AppDbContext _context;
        private readonly DB.User _user;

        public ExpensesServices(DB.AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {  //dependency injection
            _context = context;
            _user = _context.Users
                .First(u=> u.Username == httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public Expense CreateExpense(DB.Expense expense)
        {   //add the exepnse to the DB
            expense.User = _user;

            _context.Add(expense);
            //save the changes and return expense
            _context.SaveChanges();
            return (Expense)expense;
        }

        public void DeleteExpense(Expense expense)
        {
            var dbExpense = _context.Expenses.First(e => e.User.Id == _user.Id && e.ID == expense.ID);
            _context.Expenses.Remove(dbExpense);
            _context.SaveChanges();
        }
        public Expense EditExpense(Expense expense)
        {
            var dbExpense = _context.Expenses.First(e => e.User.Id == _user.Id && e.ID == expense.ID);
            dbExpense.Description = expense.Description;
            dbExpense.Amount = expense.Amount;

            _context.SaveChanges();

            return expense;
        }
        public Expense GetExpense(int id) =>
         //return the first expense that matches the ID in the expenses list
         _context.Expenses
         .Where(e => e.User.Id == _user.Id && e.ID == id).Select(e => (Expense)e)
         .First();


        public List<Expense> GetExpenses() =>
         //return the first expense that matches the ID in the expenses list
         _context.Expenses
         .Where(e => e.User.Id == _user.Id)
         .Select(e => (Expense)e)
         .ToList();

    }

}
