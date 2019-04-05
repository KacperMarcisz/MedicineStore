using MedicineStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MedicineStore.WEB.Controllers
{
    using CORE.ViewModels;
    using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> EditMedicine(MedicineDetailsViewModel model)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{model.Id}", Method.PUT);
            restClient.Execute<MedicineDetailsViewModel>(request);

            return null;
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadImage(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var restClient = new RestClient("http://localhost:5000");
                    var request = new RestRequest($"api/images/AddImageForMedicine", Method.POST);
                    request.AddParameter("model", new ImageViewModel { File = file });
                    restClient.Execute(request);
                }
            }

            return Ok();
        }


        public async Task<IActionResult> DeleteMedicine(int id)
        {
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
