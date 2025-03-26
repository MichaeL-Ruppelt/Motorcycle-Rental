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
    public class CreateMotorcycleUseCaseTest
    {
        private readonly Mock<ILogger<CreateMotorcycleUsecase>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly CreateMotorcycleUsecase _createMotorcycleUsecase;

        public CreateMotorcycleUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<CreateMotorcycleUsecase>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();

            _createMotorcycleUsecase = new CreateMotorcycleUsecase(
                _loggerMock.Object,
                _motorcycleRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenDataIsInvalid()
        {
            // Arrange
            var newMotorcycle = new Motorcycle(); // Invalid data

            // Act
            var result = await _createMotorcycleUsecase.ExecuteAsync(newMotorcycle);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenMotorcycleAlreadyExists()
        {
            // Arrange
            var newMotorcycle = new Motorcycle { LicensePlate = "ABC1234" };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByLicensePlate(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Motorcycle());

            // Act
            var result = await _createMotorcycleUsecase.ExecuteAsync(newMotorcycle);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnMotorcycle_WhenDataIsValid()
        {
            // Arrange
            var newMotorcycle = new Motorcycle
            {
                Identifier = "identifier",
                Year = 2024,
                Model = "Intruder125",
                LicensePlate = "ABC1234",
            };

            _motorcycleRepositoryMock.Setup(repo => repo.GetByLicensePlate(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Motorcycle)null);
            _motorcycleRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Motorcycle>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _createMotorcycleUsecase.ExecuteAsync(newMotorcycle);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newMotorcycle, result);
        }
    }
}
