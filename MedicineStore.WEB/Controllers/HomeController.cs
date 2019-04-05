using MedicineStore.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MedicineStore.WEB.Controllers
{
    using CORE.ViewModels;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using RestSharp;

    public class HomeController : Controller
    {
        private IValidator<AddMedicineViewModel> _addMedicineValidator;

        public HomeController(IValidator<AddMedicineViewModel> addMedicineValidator)
        {
            _addMedicineValidator = addMedicineValidator;
        }

        public async Task<IActionResult> Index()
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest("api/medicines", Method.GET);
            var result = restClient.Execute<List<MedicineHeaderViewModel>>(request).Data;

            return View(result);
        }

        public IActionResult AddMedicine()
        {
            return View(new AddMedicineViewModel());
        }

        [HttpPost]
        public IActionResult AddMedicine(AddMedicineViewModel model)
        {
            var validationResult = _addMedicineValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }

            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest("api/medicines", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(model);
            var response = restClient.Execute(request);
            
            return RedirectToAction("Index");
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
            request.RequestFormat = DataFormat.Json;
            request.AddBody(model);
            restClient.Execute<MedicineDetailsViewModel>(request);

            return RedirectToAction("Index");
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
