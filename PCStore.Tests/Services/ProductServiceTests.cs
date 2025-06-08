using Xunit;
using Moq;
using PCStore.Models;
using PCStore.Repositories;
using PCStore.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCStore.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<ICPURepository> _mockCpuRepository;
        private readonly Mock<IGPURepository> _mockGpuRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockCpuRepository = new Mock<ICPURepository>();
            _mockGpuRepository = new Mock<IGPURepository>();
            _productService = new ProductService(_mockCpuRepository.Object, _mockGpuRepository.Object);
        }

        [Fact]
        public async Task GetCPUs_ReturnsAllCPUs()
        {
            var cpus = new List<CPU> {
                new CPU { Id = 1, Name = "Intel Core", Model = "i9 14900K", Price = 900, Cores = 24, ClockSpeed = 4.8f },
                new CPU { Id = 2, Name = "AMD Ryzen", Model = "7 9700X", Price = 600, Cores = 16, ClockSpeed = 5.2f }
            };

            _mockCpuRepository.Setup(repo => repo.GetAllCPUsAsync()).ReturnsAsync(cpus);

            var result = await _productService.GetCPUsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetGPUs_ReturnsAllGPUs()
        {
            var gpus = new List<GPU> {
                new GPU { Id = 1, Name = "NVIDIA", Model = "RTX 3080", Price = 800, Memory = 10 },
                new GPU { Id = 2, Name = "AMD", Model = "RX 6800", Price = 700, Memory = 16 }
            };

            _mockGpuRepository.Setup(repo => repo.GetAllGPUsAsync()).ReturnsAsync(gpus);

            var result = await _productService.GetGPUsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductById_ReturnsCPU_WhenIdMatchesCPU()
        {
            var cpu = new CPU { Id = 1, Name = "Intel Core", Model = "i9 14900K", Price = 900, Cores = 24, ClockSpeed = 4.8f };
            _mockCpuRepository.Setup(repo => repo.GetCPUByIdAsync(1)).ReturnsAsync(cpu);

            var result = await _productService.GetProductByIdAsync(1);

            Assert.Equal(cpu, result);
        }

        [Fact]
        public async Task GetProductById_ReturnsGPU_WhenIdMatchesGPU()
        {
            var gpu = new GPU { Id = 2, Name = "NVIDIA", Model = "RTX 3080", Price = 800, Memory = 10 };
            _mockGpuRepository.Setup(repo => repo.GetGPUByIdAsync(2)).ReturnsAsync(gpu);

            var result = await _productService.GetProductByIdAsync(2);

            Assert.Equal(gpu, result);
        }

        [Fact]
        public async Task GetProductById_ReturnsNull_WhenIdNotFound()
        {
            _mockCpuRepository.Setup(repo => repo.GetCPUByIdAsync(It.IsAny<int>())).ReturnsAsync((CPU?)null);
            _mockGpuRepository.Setup(repo => repo.GetGPUByIdAsync(It.IsAny<int>())).ReturnsAsync((GPU?)null);

            var result = await _productService.GetProductByIdAsync(3);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsTrue_WhenCPUDeleted()
        {
            _mockCpuRepository.Setup(repo => repo.GetCPUByIdAsync(1)).ReturnsAsync(new CPU { Id = 1 });
            _mockCpuRepository.Setup(repo => repo.DeleteCPUAsync(1)).Returns(Task.CompletedTask);

            var result = await _productService.DeleteProductAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsTrue_WhenGPUDeleted()
        {
            _mockGpuRepository.Setup(repo => repo.GetGPUByIdAsync(2)).ReturnsAsync(new GPU { Id = 2 });
            _mockGpuRepository.Setup(repo => repo.DeleteGPUAsync(2)).Returns(Task.CompletedTask);

            var result = await _productService.DeleteProductAsync(2);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsFalse_WhenProductNotFound()
        {
            _mockCpuRepository.Setup(repo => repo.GetCPUByIdAsync(It.IsAny<int>())).ReturnsAsync((CPU?)null);
            _mockGpuRepository.Setup(repo => repo.GetGPUByIdAsync(It.IsAny<int>())).ReturnsAsync((GPU?)null);

            var result = await _productService.DeleteProductAsync(3);

            Assert.False(result);
        }
    }
}