
namespace MedicineStore.TESTS.Controllers
{
    using AutoMapper;
    using MedicineStore.API.Controllers;
    using MedicineStore.API.Data;
    using MedicineStore.API.Models;
    using MedicineStore.CORE.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class MedicinesControllerTests
    {
        [Fact]
        public async Task Get_CorrectData_MedicineHeaderViewModelListAsync()
        {
            // Arrange
            var mock = new Mock<IMedicineStoreRepository>();
            var mockMapper = new Mock<IMapper>();

            mock.Setup(x => x.GetAllMedicinesAsync())
                .Returns(() => Task.FromResult(GetMedicineList()));
            mockMapper.Setup(x => x.Map<IEnumerable<MedicineHeaderViewModel>>(It.IsAny<List<Medicine>>()))
                .Returns(() => GetMedicineHeaderViewModel());

            var controller = new MedicinesController(mock.Object, mockMapper.Object);

            // Act
            var actionResult = await controller.Get() as OkObjectResult;
            var result = actionResult.Value as List<MedicineHeaderViewModel>;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetDetails_CorrectData_MedicineDetailsViewModel()
        {
            // Arrange
            var mock = new Mock<IMedicineStoreRepository>();
            var mockMapper = new Mock<IMapper>();

            mock.Setup(x => x.GetMedicineAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(GetMedicineList(1).FirstOrDefault()));
            mockMapper.Setup(x => x.Map<MedicineDetailsViewModel>(It.IsAny<Medicine>()))
                .Returns(() => GetMedicineDetailsViewModel());

            var controller = new MedicinesController(mock.Object, mockMapper.Object);

            // Act
            var actionResult = await controller.GetDetails(It.IsAny<int>()) as OkObjectResult;
            var result = actionResult.Value as MedicineDetailsViewModel;

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);
            Assert.NotNull(result);
        }

        private List<Medicine> GetMedicineList(int id = 0)
        {
            var list = new List<Medicine>
            {
                new Medicine
                {
                    Id = 1,
                    Name = "Test1",
                    Description = "Description1",
                    GrossPrice = 50,
                    SpecialGrossPrice = 20,
                    IsActive = true,
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 2,
                    Name = "Test2",
                    Description = "Description2",
                    GrossPrice = 50,
                    IsActive = true,
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 3,
                    Name = "Test3",
                    Description = "Description3",
                    GrossPrice = 50,
                    SpecialGrossPrice = 20,
                    IsActive = false,
                    IsDeleted = false
                },
                new Medicine
                {
                    Id = 4,
                    Name = "Test4",
                    Description = "Description4",
                    GrossPrice = 50,
                    SpecialGrossPrice = 20,
                    IsActive = true,
                    IsDeleted = true
                },
            };

            if (id != 0)
            {
                return list.Where(x => x.Id == id).ToList();
            }

            return list;
        }

        private IEnumerable<MedicineHeaderViewModel> GetMedicineHeaderViewModel(int id = 0)
        {
            var list = new List<MedicineHeaderViewModel>
            {
                new MedicineHeaderViewModel
                {
                    Id = 1,
                    Name = "Test1",
                    GrossPrice = 50,
                    SpecialGrossPrice = 20
                },
                new MedicineHeaderViewModel
                {
                    Id = 2,
                    Name = "Test2",
                    GrossPrice = 50,
                    SpecialGrossPrice = 0
                },
            };

            if (id != 0)
            {
                return list.Where(x => x.Id == id).ToList();
            }

            return list;
        }

        private MedicineDetailsViewModel GetMedicineDetailsViewModel()
        {
            return new MedicineDetailsViewModel
            {
                Id = 1,
                Name = "Test1",
                GrossPrice = 50,
                SpecialGrossPrice = 20,
                Description = "Test1"
            };
        }
    }
}
