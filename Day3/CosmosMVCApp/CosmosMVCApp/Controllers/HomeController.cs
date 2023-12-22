using CosmosMVCApp.Config;
using CosmosMVCApp.Models;
using CosmosMVCApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CosmosMVCApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ConfigModel _configOptions;
		public HomeController(ILogger<HomeController> logger, ConfigModel options)
		{
			_logger = logger;
			_configOptions = options;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public async Task<IActionResult> ViewTodos()
		{

			TodoService db = new TodoService(_configOptions.CosomosdbConnectionString, _configOptions.CosmosdbDatabaseName, _configOptions.CosmosdbContainerName);
			List<TodoVM> todos = await db.GetTodosAsync();

			return View(todos);

		}

		[HttpGet]
        public IActionResult AddTodo()
        {
            TodoVM vm = new TodoVM();
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> AddTodo(TodoVM vm)
        {

            TodoService db = new TodoService(_configOptions.CosomosdbConnectionString, _configOptions.CosmosdbDatabaseName, _configOptions.CosmosdbContainerName);
            bool result = await db.AddTodo(vm);

            if (result)
                return RedirectToAction("ViewTodos");
            else
                return NotFound();

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
