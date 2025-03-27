using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.Entities;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using Xunit;

namespace MotorCycleRentail.Test.Unit.Application.Usecases.Motorcycles
{
    public class RemoveMotorcycleUseCaseTest
    {
        private readonly Mock<ILogger<RemoveMotorcycleUsecase>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly RemoveMotorcycleUsecase _removeMotorcycleUsecase;

        public RemoveMotorcycleUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<RemoveMotorcycleUsecase>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            _removeMotorcycleUsecase = new RemoveMotorcycleUsecase(
                _loggerMock.Object,
                _motorcycleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenIdIsNullOrEmpty()
        {
            // Arrange
            string id = null;

            // Act
            var result = await _removeMotorcycleUsecase.ExecuteAsync(id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenIdIsInvalidGuid()
        {
            // Arrange
            string id = "invalid-guid";

            // Act
            var result = await _removeMotorcycleUsecase.ExecuteAsync(id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _removeMotorcycleUsecase.ExecuteAsync(id);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrue_WhenMotorcycleIsRemoved()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var motorcycle = new Motorcycle { Id = Guid.Parse(id) };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(motorcycle);
            _motorcycleRepositoryMock.Setup(repo => repo.DeleteById(motorcycle.Id, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _removeMotorcycleUsecase.ExecuteAsync(id);

            // Assert
            Assert.True(result);
        }
    }
}
