 
using Expenses.Core;
using Expenses.DB;
using Microsoft.AspNetCore.Mvc;
 
namespace Expenses.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpensesController : ControllerBase
    {
        private IExpensesServices _expensesServices;
        public ExpensesController(IExpensesServices expensesServices)
        {
            _expensesServices = expensesServices;
        }

        //get all the exenses
        [HttpGet]
        public IActionResult GetExpenses()
        {
            return Ok(_expensesServices.GetExpenses());
        }

        //get all the exenses with an ID
        [HttpGet("{id}",  Name = "GetExpense")]
        public IActionResult GetExpense(int id)
        {
            return Ok(_expensesServices.GetExpense(id));
        }

        [HttpPost]
        public IActionResult CreateExpense(Expense expense)
        {
            var newExpense = _expensesServices.CreateExpense(expense);
            return CreatedAtRoute("GetExpense",  new {newExpense.ID}, newExpense);
        }

        //delete the expense with an ID
        [HttpDelete]
        public IActionResult DeleteExpense(Expense expense)
        {
             _expensesServices.DeleteExpense(expense);
            return Ok(_expensesServices.GetExpenses());
        }

        [HttpPut]
        public IActionResult EditExpense(Expense expense)
        {
            return Ok(_expensesServices.EditExpense(expense));
        }

    }
}
