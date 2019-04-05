using Datatables.Sample.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Datatables.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddEditTodo(int id = 0)
        {
            //if (id == 0)
            //{
                return View(new ToDoViewModel());
            //}
            //else
            //{
            //    return View(_context.Todo.Where(x => x.todoId.Equals(id)).FirstOrDefault());
            //}

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
