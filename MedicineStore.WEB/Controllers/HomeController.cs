namespace MedicineStore.WEB.Controllers
{
    using CORE.ViewModels;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using RestSharp;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IValidator<AddMedicineViewModel> _addMedicineValidator;

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
            var request = new RestRequest($"api/medicines/edit/{id}", Method.GET);
            var result = restClient.Execute<EditMedicineViewModel>(request).Data;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(MedicineDetailsViewModel model)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{model.Id}", Method.PUT)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddBody(model);
            restClient.Execute<MedicineDetailsViewModel>(request);

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
                restClient.Execute(request);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/{id}", Method.DELETE);
            restClient.Execute(request);

            return RedirectToAction("Index");
        }

        public ActionResult SetMainImage(int medicineId, string imagePublicId)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/medicines/mainImage/{medicineId}/{imagePublicId}", Method.POST);
            restClient.Execute(request);

            return RedirectToAction("EditMedicine", new { id = medicineId });
        }

        public ActionResult DeleteImage(int medicineId, string imagePublicId)
        {
            var restClient = new RestClient("http://localhost:5000");
            var request = new RestRequest($"api/images/{imagePublicId}", Method.DELETE);
            restClient.Execute(request);

            return RedirectToAction("EditMedicine", new { id = medicineId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
