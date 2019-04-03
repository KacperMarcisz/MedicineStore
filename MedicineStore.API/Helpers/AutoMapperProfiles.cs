namespace MedicineStore.API.Helpers
{
    using AutoMapper;
    using CORE.ViewModels;
    using Models;

    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Medicine, MedicineHeaderViewModel>();
        }
    }
}
