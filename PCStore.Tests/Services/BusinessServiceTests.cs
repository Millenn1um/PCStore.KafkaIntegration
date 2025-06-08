using Moq;
using PCStore.Models;
using PCStore.Services;
using Xunit;

namespace PCStore.Tests.Services
{
    public class BusinessServiceTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly BusinessService _businessService;

        public BusinessServiceTests()
        {
            _mockProductService = new Mock<IProductService>();
            _businessService = new BusinessService(_mockProductService.Object);
        }

        private void SetupProductMocks(List<CPU> cpus, List<GPU> gpus)
        {
            _mockProductService.Setup(ps => ps.GetCPUsAsync()).ReturnsAsync(cpus);
            _mockProductService.Setup(ps => ps.GetGPUsAsync()).ReturnsAsync(gpus);
        }

        [Fact]
        public async Task GetTotalInventoryValue_Returns_CorrectSum()
        {
            // Arrange
            var cpus = new List<CPU>
            {
                new CPU { Id = 1, Name = "Intel Core i7", Model = "14700K", Price = 300m, Cores = 8, ClockSpeed = 4.6f },
                new CPU { Id = 2, Name = "AMD Ryzen 5", Model = "7600X", Price = 200m, Cores = 6, ClockSpeed = 4.2f  }
            };

            var gpus = new List<GPU>
            {
                new GPU { Id = 1, Name = "NVIDIA", Model = "RTX 4060 Ti", Price = 400m },
                new GPU { Id = 2, Name = "AMD Radeon", Model = "RX 7800 XT", Price = 500m }
            };

            SetupProductMocks(cpus, gpus);

            // Act
            var totalValue = await _businessService.GetTotalInventoryValueAsync();

            // Assert
            Assert.Equal(1400m, totalValue);
        }

        [Fact]
        public async Task GetTotalProductsCount_Returns_CorrectCount()
        {
            // Arrange
            var cpus = new List<CPU> { new CPU { Id = 1 }, new CPU { Id = 2 } };
            var gpus = new List<GPU> { new GPU { Id = 1 }, new GPU { Id = 2 }, new GPU { Id = 3 } };

            SetupProductMocks(cpus, gpus);

            // Act
            var productCount = await _businessService.GetTotalProductsCountAsync();

            // Assert
            Assert.Equal(5, productCount);
        }

        [Fact]
        public async Task GetTotalInventoryValue_Returns_Zero_WhenNoProducts()
        {
            // Arrange
            var cpus = new List<CPU>();
            var gpus = new List<GPU>();

            SetupProductMocks(cpus, gpus);

            // Act
            var totalValue = await _businessService.GetTotalInventoryValueAsync();

            // Assert
            Assert.Equal(0m, totalValue);
        }

        [Fact]
        public async Task GetTotalProductsCount_Returns_Zero_WhenNoProducts()
        {
            // Arrange
            var cpus = new List<CPU>();
            var gpus = new List<GPU>();

            SetupProductMocks(cpus, gpus);

            // Act
            var productCount = await _businessService.GetTotalProductsCountAsync();

            // Assert
            Assert.Equal(0, productCount);
        }
    }
}