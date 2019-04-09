namespace MedicineStore.API.Controllers
{
    using AutoMapper;
    using CORE.ViewModels;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class MedicinesController : ControllerBase
    {
        private readonly IMedicineStoreRepository _repo;
        private readonly IMapper _mapper;

        public MedicinesController(IMedicineStoreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var medicinesFromRepo = await _repo.GetAllMedicinesAsync();
            var medicines = _mapper.Map<IEnumerable<MedicineHeaderViewModel>>(medicinesFromRepo);

            return Ok(medicines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var medicineFromRepo = await _repo.GetMedicineAsync(id);
            var medicine = _mapper.Map<MedicineDetailsViewModel>(medicineFromRepo);

            return Ok(medicine);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> GetEdit(int id)
        {
            var medicineFromRepo = await _repo.GetMedicineAsync(id);
            var medicine = _mapper.Map<EditMedicineViewModel>(medicineFromRepo);

            return Ok(medicine);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddMedicineViewModel model)
        {
            var medicine = _mapper.Map<Medicine>(model);
            await _repo.AddMedicineAsync(medicine);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EditMedicineViewModel model)
        {
            var medicineFromRepo = await _repo.GetMedicineAsync(id);
            var imagesFromRepo = medicineFromRepo.Images.ToList();
            _mapper.Map(model, medicineFromRepo);
            medicineFromRepo.Images = imagesFromRepo;

            if (await _repo.SaveAllAsync())
                return NoContent();

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);

            return Ok();
        }

        [HttpPost("mainImage/{medicineId}/{imageId}")]
        public async Task<IActionResult> SetMainImage(int medicineId, string imageId)
        {
            var imageFromRepo = await _repo.GetImageAsync(medicineId, imageId);
            var currentMainImage = await _repo.GetMainImageAsync(medicineId);

            if (currentMainImage != null)
            {
                currentMainImage.IsMain = false;
            }

            imageFromRepo.IsMain = true;

            if (await _repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Could not set image to main");
        }

        [HttpPost("migrateData")]
        public async Task<IActionResult> MigrateMedicines(IEnumerable<AddMedicineViewModel> model)
        {
            var medicines = _mapper.Map<List<Medicine>>(model);
            await _repo.AddMedicinesRange(medicines);

            if (await _repo.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("search/{searchingPhrase}")]
        public async Task<IActionResult> Search(string searchingPhrase)
        {
            var medicinesFromRepo = await _repo.GetMedicinesAsync(searchingPhrase);
            var medicines = _mapper.Map<IEnumerable<MedicineHeaderViewModel>>(medicinesFromRepo);
            
            return Ok(medicines);
        }
    }
}
