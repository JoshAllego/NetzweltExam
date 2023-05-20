using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;


namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


      
        public ActionResult LogIn()
        {

            return View();
        }

        

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
       public async Task<JsonResult> GetData()
        {
            JsonResult _res = null;


            return _res;
        }
    }
}