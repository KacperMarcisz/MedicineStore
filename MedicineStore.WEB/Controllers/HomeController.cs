namespace MedicineStore.WEB.Controllers
{
    using CORE.ViewModels;
    using CsvHelper;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using RestSharp;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IValidator<AddMedicineViewModel> _addMedicineValidator;
        private readonly IValidator<EditMedicineViewModel> _editMedicineValidator;

        public HomeController(IValidator<AddMedicineViewModel> addMedicineValidator, IValidator<EditMedicineViewModel> editMedicineValidator)
        {
            _addMedicineValidator = addMedicineValidator;
            _editMedicineValidator = editMedicineValidator;
        }

        public async Task<IActionResult> Index()
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest("api/medicines", Method.GET);
            var result = (await restClient.ExecuteTaskAsync<List<MedicineHeaderViewModel>>(request)).Data;

            return View(result);
        }

        public IActionResult AddMedicine()
        {
            return View(new AddMedicineViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicine(AddMedicineViewModel model)
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
            await restClient.ExecuteTaskAsync(request);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MedicineDetails(int id)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{id}", Method.GET);
            var result = (await restClient.ExecuteTaskAsync<MedicineDetailsViewModel>(request)).Data;

            return View(result);
        }

        public async Task<IActionResult> EditMedicine(int id)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/edit/{id}", Method.GET);
            var result = (await restClient.ExecuteTaskAsync<EditMedicineViewModel>(request)).Data;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(EditMedicineViewModel model)
        {
            var validationResult = _editMedicineValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model);
            }

            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{model.Id}", Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(model);
            await restClient.ExecuteTaskAsync<EditMedicineViewModel>(request);

            return RedirectToAction("Index");
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadImage(int id, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                byte[] data;
                using (var br = new BinaryReader(file.OpenReadStream()))
                {
                    data = br.ReadBytes((int)file.OpenReadStream().Length);
                }

                var restClient = new RestClient("http://localhost:5000");
                var request = new RestRequest($"api/images/{id}", Method.POST);
                request.AddFileBytes("imageName", data, "ImageFileName");
                request.AddHeader("Content-Type", "multipart/form-data");
                await restClient.ExecuteTaskAsync(request);
            }

            return RedirectToAction("EditMedicine", new { id });
        }

        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{id}", Method.DELETE);
            await restClient.ExecuteTaskAsync(request);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SetMainImage(int medicineId, string imagePublicId)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/mainImage/{medicineId}/{imagePublicId}", Method.POST);
            await restClient.ExecuteTaskAsync(request);

            return RedirectToAction("EditMedicine", new { id = medicineId });
        }

        public async Task<IActionResult> DeleteImage(int medicineId, string imagePublicId)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/images/{imagePublicId}", Method.DELETE);
            await restClient.ExecuteTaskAsync(request);

            return RedirectToAction("EditMedicine", new { id = medicineId });
        }

        public ActionResult MigrateMedicines()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MigrateMedicines(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var reader = new StreamReader(file.OpenReadStream());
                var csvReader = new CsvReader(reader);
                var records = csvReader.GetRecords<MigrateMedicinesViewModel>();

                var model = records.Select(x => new AddMedicineViewModel
                {
                    GrossPrice = decimal.Parse(x.GrossPrice),
                    Name = x.Name,
                    Description = x.Description,
                    SpecialGrossPrice = decimal.Parse(x.SpecialGrossPrice)
                }).ToList();

                var restClient = new RestClient("http://localhost:5000");
                var request = new RestRequest($"api/medicines/migrateData", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(model);
                await restClient.ExecuteTaskAsync(request);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchingPhrase)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/search/{searchingPhrase}", Method.GET);
            var result = (await restClient.ExecuteTaskAsync<List<MedicineHeaderViewModel>>(request)).Data;

            return View("Index", result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
