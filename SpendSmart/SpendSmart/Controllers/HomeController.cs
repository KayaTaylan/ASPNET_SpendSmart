using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly SpendSmartDbContext _context; //Dependency Injection

        public HomeController(ILogger<HomeController> logger, SpendSmartDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _context.Expenses.ToList(); //Take Database the Query (ALL VALUES) and convert it to a list

            var totalExpenses = allExpenses.Sum(x => x.Value);
            ViewBag.Expenses = totalExpenses; //Inside the View, we can access whatever is inside this Bag

            return View(allExpenses); //Returns the View with EXACTLY the same name as the method ("Expenses")
        }

        public IActionResult CreateEditExpense(int? id) //can be null, if we add a new expense
        {
            if(id != null)
            {
                //editing -> load an expense by id
                var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id);
                return View(expenseInDb);
            }
            
            return View();
        }

        public IActionResult DeleteExpense(int id) //Id required
        {
            var expenseInDb = _context.Expenses.SingleOrDefault(expense => expense.Id == id); //Take the first one we find where x.Id equals id and return it as x
            _context.Expenses.Remove(expenseInDb);
            _context.SaveChanges();
            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if(model.Id == 0)
            {
                //Create
                _context.Expenses.Add(model); //Add the module to the Database
            }
            else
            {
                //Editing
                _context.Expenses.Update(model); //Update an existing entry
            }
            
            _context.SaveChanges();       //ALWAYS Save the changes, otherwise modification is lost

            return RedirectToAction("Expenses"); //Calls the Expenses() method of this class
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
