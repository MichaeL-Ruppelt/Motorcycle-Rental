using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.Entities;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using Xunit;

namespace MotorCycleRentail.Test.Unit.Application.Usecases.Motorcycles
{
    public class UpdateMotorcycleUseCaseTest
    {
        private readonly Mock<ILogger<UpdateMotorcycleUsecase>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly UpdateMotorcycleUsecase _updateMotorcycleUsecase;

        public UpdateMotorcycleUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<UpdateMotorcycleUsecase>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            _updateMotorcycleUsecase = new UpdateMotorcycleUsecase(
                _loggerMock.Object,
                _motorcycleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenIdIsNullOrEmpty()
        {
            // Arrange
            string id = null;
            var request = new UpdateMotorcycleRequest { LicensePlate = "ABC1234" };

            // Act
            var result = await _updateMotorcycleUsecase.ExecuteAsync(id, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenIdIsInvalidGuid()
        {
            // Arrange
            string id = "invalid-guid";
            var request = new UpdateMotorcycleRequest { LicensePlate = "ABC1234" };

            // Act
            var result = await _updateMotorcycleUsecase.ExecuteAsync(id, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenLicensePlateIsNullOrEmpty()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var request = new UpdateMotorcycleRequest { LicensePlate = null };

            // Act
            var result = await _updateMotorcycleUsecase.ExecuteAsync(id, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var request = new UpdateMotorcycleRequest { LicensePlate = "ABC1234" };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _updateMotorcycleUsecase.ExecuteAsync(id, request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrue_WhenDataIsValid()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            var request = new UpdateMotorcycleRequest { LicensePlate = "ABC1234" };
            var motorcycle = new Motorcycle { Id = Guid.Parse(id) };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(motorcycle);
            _motorcycleRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Motorcycle>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _updateMotorcycleUsecase.ExecuteAsync(id, request);

            // Assert
            Assert.True(result);
        }
    }
}


