using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string getTerritoriesEndPoint = @"https://netzwelt-devtest.azurewebsites.net/Territories/All";


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
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionModel.Name))) throw new Exception("Session expired");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        

        [HttpGet]
       public async Task<JsonResult> GetData()
        {
            JsonResult _res = null; 
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionModel.Name))) throw new Exception("Session expired!");
                    if (!ModelState.IsValid) { throw new Exception("Model is empty!"); };
                 
                HttpClientHandler _handler = new HttpClientHandler();
                _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPoloc) => { return true; };

                HttpClient _client = new HttpClient(_handler);
                var _response = await _client.GetAsync(getTerritoriesEndPoint);
                var ppe = @_response.Content.ReadAsStringAsync().Result.ToString();  
 
                var territories = JsonConvert.DeserializeObject<Root>(ppe);

                var listOfMainTerritory = territories.data.Where(a => string.IsNullOrEmpty(a.parent)).Select(a => a.id).Distinct().ToList();


                List<MainTerritory> mainTerritories = new List<MainTerritory>();

                foreach(var i in listOfMainTerritory)
                {
                    var m1 = new MainTerritory();
                    m1.Name = territories.data.Where(a => a.id == i).Select(a => a.name).FirstOrDefault();

                    var mxx = territories.data.Where(a => a.parent == i).ToList();
                  
                    foreach(var j in mxx)
                    {
                        var m2 = new SubTerritory();
                        m2.Name = j.name;
                        var mxm = territories.data.Where(a => a.parent == j.id).ToList();
                        foreach(var z in mxm)
                        {
                            m2.SubTerritories.Add(z.name);
                        }
                        m1.SubTerritories.Add(m2);
                    } 

                    mainTerritories.Add(m1);
                }
                
                _res = new JsonResult(mainTerritories);
            }
            catch (Exception ex)
            {

                _res = new JsonResult(ex.Message);
            }

            return _res;
        }
    }
}