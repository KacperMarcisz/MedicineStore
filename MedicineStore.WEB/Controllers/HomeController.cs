using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MedicineStore.WEB.Models;

namespace MedicineStore.WEB.Controllers
{
    using System.Net.Http;
    using CORE.ViewModels;
    using RestSharp;

    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest("api/medicines", Method.GET);
            var result = restClient.Execute<List<MedicineHeaderViewModel>>(request).Data;
            
            return View(result);
        }

        public IActionResult AddMedicine()
        {
            return View();
        }

        public async Task<IActionResult> MedicineDetails(int id)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{id}", Method.GET);
            var result = restClient.Execute<MedicineDetailsViewModel>(request).Data;
            
            return View(result);
        }

        public async Task<IActionResult> EditMedicine(int id)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{id}", Method.GET);
            var result = restClient.Execute<MedicineDetailsViewModel>(request).Data;
            
            return View(result);
        }


        public async Task<IActionResult> DeleteMedicine(int id)
        {

        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
