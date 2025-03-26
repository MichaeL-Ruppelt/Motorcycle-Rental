using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using Xunit;
using MotorCycleRentail.Application.Services;
using MotorCycleRentail.Domain.Entities;

namespace MotorCycleRentail.Test.Unit.Application.Usecases.Motorcycles
{
    public class SendMotocycleEventUseCaseTest
    {
        private readonly Mock<ILogger<SendMotocycleEventUseCase>> _loggerMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly Mock<IMessagePublisherService> _messagePublisherMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly SendMotocycleEventUseCase _sendMotocycleEventUseCase;

        public SendMotocycleEventUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<SendMotocycleEventUseCase>>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _messagePublisherMock = new Mock<IMessagePublisherService>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(config => config["BusinessRules:MaxMotorcycleYear"]).Returns("2022");

            _sendMotocycleEventUseCase = new SendMotocycleEventUseCase(
                _loggerMock.Object,
                _motorcycleRepositoryMock.Object,
                _messagePublisherMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenMotorcycleYearIsInvalid()
        {
            // Arrange
            var request = new MotorcycleRequest { Year = 2021 };

            // Act
            var result = await _sendMotocycleEventUseCase.ExecuteAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenDataIsInvalid()
        {
            // Arrange
            var request = new MotorcycleRequest { Year = 2023, LicensePlate = "ABC1234" };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByLicensePlate(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _sendMotocycleEventUseCase.ExecuteAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenMotorcycleAlreadyExists()
        {
            // Arrange
            var request = new MotorcycleRequest
            {
                Year = 2023,
                LicensePlate = "ABC1234",
                Identifier = "identifier",
                Model = "model"
            };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByLicensePlate(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Motorcycle());

            // Act
            var result = await _sendMotocycleEventUseCase.ExecuteAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrue_WhenDataIsValid()
        {
            // Arrange
            var request = new MotorcycleRequest
            {
                Year = 2023,
                LicensePlate = "ABC1234",
                Identifier = "identifier",
                Model = "model"
            };
            _motorcycleRepositoryMock.Setup(repo => repo.GetByLicensePlate(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Motorcycle)null);
            _messagePublisherMock.Setup(service => service.SendMessage(It.IsAny<Motorcycle>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sendMotocycleEventUseCase.ExecuteAsync(request);

            // Assert
            Assert.True(result);
        }
    }
}

