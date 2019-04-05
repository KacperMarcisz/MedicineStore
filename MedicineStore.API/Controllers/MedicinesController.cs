namespace MedicineStore.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using CORE.ViewModels;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models;

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
            var medicinesFromRepo = await _repo.GetAllMedicines();
            var medicines = _mapper.Map<IEnumerable<MedicineHeaderViewModel>>(medicinesFromRepo);

            return Ok(medicines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var medicineFromRepo = await _repo.GetMedicine(id);
            var medicine = _mapper.Map<MedicineDetailsViewModel>(medicineFromRepo);

            return Ok(medicine);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MedicineDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var medicine = _mapper.Map<Medicine>(model);
            //await _repo.EditMedicine(medicine);

            return Ok();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPatch]
        public void Patch()
        {

        }
    }
}
