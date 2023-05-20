using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Exam.Controllers
{
    public class AccountController : Controller
    {

        string signInEndPoint = @"https://netzwelt-devtest.azurewebsites.net/Account/SignIn";
       

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> LogIn(LogInModel model)
        {
            LogInResponseModel? _responseModel = null;
            try
            {
                if (!ModelState.IsValid) { throw new Exception("Model is empty!"); };
                if (string.IsNullOrEmpty(model.userName)) throw new Exception("User id is empty!");
                if (string.IsNullOrEmpty(model.password)) throw new Exception("Password is empty!");


                HttpClientHandler _handler = new HttpClientHandler();
                _handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPoloc) => { return true; };

                HttpClient _client = new HttpClient(_handler);

                string _serialize = JsonConvert.SerializeObject(model);
                var _stringContent = new StringContent(_serialize, Encoding.UTF8, "application/json");
                var _response = await _client.PostAsync(signInEndPoint, _stringContent);
                _responseModel = await _response.Content.ReadFromJsonAsync<LogInResponseModel>();

               

                    if (_responseModel != null)
                    {
                        
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Account");
                    }
                 
            }
            catch (Exception)
            {

                throw;
            }
             
            return View();
        }
    }
}
