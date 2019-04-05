namespace MedicineStore.API.Controllers
{
    using AutoMapper;
    using CORE.ViewModels;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Collections.Generic;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddMedicineViewModel model)
        {
            var medicine = _mapper.Map<Medicine>(model);
            await _repo.AddMedicineAsync(medicine);
            
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MedicineDetailsViewModel model)
        {
            var medicineFromRepo = await _repo.GetMedicineAsync(id);
            _mapper.Map(model, medicineFromRepo);

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

        // [HttpPatch]
        // public void Patch()
        // {

        // }
    }
}
