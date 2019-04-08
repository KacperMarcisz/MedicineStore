namespace MedicineStore.API.Helpers
{
    using AutoMapper;
    using CORE.ViewModels;
    using Models;
    using System.Linq;

    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Medicine, MedicineHeaderViewModel>()
                .ForMember(dest => dest.ImageUrl, opt =>
                {
                    opt.MapFrom(src => src.Images.FirstOrDefault(x => x.IsMain).Url);
                });
            CreateMap<Medicine, MedicineDetailsViewModel>()
                .ForMember(dest => dest.ImageUrl, opt =>
                {
                    opt.MapFrom(src => src.Images.FirstOrDefault(x => x.IsMain).Url);
                });
            CreateMap<AddMedicineViewModel, Medicine>();
            CreateMap<MedicineDetailsViewModel, Medicine>();
            CreateMap<EditMedicineViewModel, Medicine>()
                .ForMember(dest => dest.Images, opt =>
                {
                    opt.MapFrom(src => src.Images.ToList());
                });
            CreateMap<Medicine, EditMedicineViewModel>()
                .ForMember(dest => dest.Images, opt =>
                {
                    opt.MapFrom(src => src.Images);
                });
            CreateMap<Image, ImageViewModel>();
            CreateMap<ImageViewModel, Image>();
        }
    }
}
