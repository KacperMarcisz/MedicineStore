using Microsoft.AspNetCore.Mvc;

namespace MedicineStore.API.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using Helpers;
    using Microsoft.Extensions.Options;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using CORE.ViewModels;
    using Data;
    using Microsoft.EntityFrameworkCore.Internal;
    using Models;
    using System.Linq;

    [Route("api/{controller}")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private IMedicineStoreRepository _repo;
        private IMapper _mapper { get; }
        private IOptions<CloudinarySettings> _cloudinaryConfig { get; }
        private readonly Cloudinary _cloudinary;

        public ImagesController(IMedicineStoreRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var imageFromRepo = await _repo.GetImage(id);
            var image = _mapper.Map<ImageViewModel>(imageFromRepo);

            return Ok(image);
        }

        [Route("{medicineId}/AddImageForMedicine")]
        [HttpPost]
        public async Task<IActionResult> AddImageForMedicine(int medicineId, ImageViewModel model)
        {
            var medicine = await _repo.GetMedicine(medicineId);

            var file = model.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            model.Url = uploadResult.Uri.ToString();
            model.PublicId = uploadResult.PublicId;

            var image = _mapper.Map<Image>(model);
            image.MedicineId = medicineId;

            if (!medicine.Images.Any(x => x.IsMain))
            {
                image.IsMain = true;
            }

            medicine.Images.Add(image);

            if (await _repo.SaveAll())
            {
                var imageToReturn = _mapper.Map<ImageViewModel>(image);
                return CreatedAtRoute("GetImage", new { id = image.Id }, imageToReturn);
            }

            return BadRequest("Could not add image");
        }
    }
}