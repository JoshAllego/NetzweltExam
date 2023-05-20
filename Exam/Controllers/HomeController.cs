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

        string getTerritoriesEndPoint = @"https://netzwelt-devtest.azurewebsites.net/Territories/All";


        [HttpGet]
       public async Task<JsonResult> GetData()
        {
            JsonResult _res = null; 
            try
            {
                if (!ModelState.IsValid) { throw new Exception("Model is empty!"); };
                 
                HttpClientHandler _handler = new HttpClientHandler();
                _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPoloc) => { return true; };

                HttpClient _client = new HttpClient(_handler);
                var _response = await _client.GetAsync(getTerritoriesEndPoint);
                var ppe = @_response.Content.ReadAsStringAsync().Result.ToString();  

                JObject de = JObject.Parse(ppe);
                 
                List<Territory> territories = new List<Territory>();


                if(de["data"] != null)
                {
                    foreach (var i in de["data"])
                    {
                        territories.Add(new Territory
                        {
                            id = i["id"] != null ? i["id"].ToString() : "",
                            name = i["name"] != null ? i["name"].ToString() : "".ToString(),
                            parent = i["parent"] != null ? i["parent"].ToString() : "".ToString()
                        });
                    }
                }
                
                var listOfMainTerritory = territories.Where(a => string.IsNullOrEmpty(a.parent)).Select(a => a.id).Distinct().ToList();


                List<MainTerritory> mainTerritories = new List<MainTerritory>();

                foreach(var i in listOfMainTerritory)
                {
                    var m1 = new MainTerritory();
                    m1.Name = territories.Where(a => a.id == i).Select(a => a.name).FirstOrDefault();

                    var mxx = territories.Where(a => a.parent == i).ToList();
                  
                    foreach(var j in mxx)
                    {
                        var m2 = new SubTerritory();
                        m2.Name = j.name;
                        var mxm = territories.Where(a => a.parent == j.id).ToList();
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
            catch (Exception)
            {

                throw;
            }

            return _res;
        }
    }
}