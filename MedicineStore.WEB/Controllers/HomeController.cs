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

    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            IEnumerable<MedicineHeaderViewModel> model = null;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("http://localhost:5000/api/medicines");

                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsAsync<IEnumerable<MedicineHeaderViewModel>>();
                }
            }

            return View(model);
        }

        public IActionResult AddMedicine()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
