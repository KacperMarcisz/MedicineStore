namespace MedicineStore.API.Controllers
{
    using AutoMapper;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using CORE.ViewModels;
    using Data;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Models;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("api/{controller}")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMedicineStoreRepository _repo;
        private IMapper _mapper { get; }
        private IOptions<CloudinarySettings> _cloudinaryConfig { get; }
        private readonly Cloudinary _cloudinary;

        public ImagesController(IMedicineStoreRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            var acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var imageFromRepo = await _repo.GetImageAsync(id);
            var image = _mapper.Map<ImageViewModel>(imageFromRepo);

            return Ok(image);
        }

        [HttpPost("{medicineId}")]
        public async Task<IActionResult> AddImageForMedicine(int medicineId)
        {
            if (Request.HasFormContentType)
            {
                var form = Request.Form;
                foreach (var formFile in form.Files)
                {
                    var medicine = await _repo.GetMedicineAsync(medicineId);
                    var model = new ImageViewModel();

                    var file = formFile;
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

                    if (await _repo.SaveAllAsync())
                    {
                        var imageToReturn = _mapper.Map<ImageViewModel>(image);
                        return CreatedAtRoute("GetImage", new { id = image.Id }, imageToReturn);
                    }
                }
            }

            return BadRequest("Could not add image");
        }

        [HttpDelete("{imagePublicId}")]
        public async Task<IActionResult> DeleteImage(string imagePublicId)
        {
            var imageFromRepo = await _repo.GetImageAsync(imagePublicId);
            if (imageFromRepo == null)
                return NotFound();
            
            var deleteParams = new DeletionParams(imageFromRepo.PublicId);
            var result = _cloudinary.Destroy(deleteParams);

            if (result.Result == "ok")
                _repo.Delete(imageFromRepo);

            if (await _repo.SaveAllAsync())
                return Ok();

            return BadRequest("Failed to delete the image");
        }
    }
}