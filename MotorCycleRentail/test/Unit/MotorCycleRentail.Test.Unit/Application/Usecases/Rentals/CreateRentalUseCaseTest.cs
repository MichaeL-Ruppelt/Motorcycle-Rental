using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using Xunit;
using MotorCycleRentail.Domain.Entities;

namespace MotorCycleRentail.Test.Unit.Application.Usecases.Rentals
{
    public class CreateRentalUseCaseTest
    {
        private readonly Mock<ILogger<CreateRentalUseCase>> _loggerMock;
        private readonly Mock<ICourierRepository> _courierRepositoryMock;
        private readonly Mock<IRentalRepository> _rentalRepositoryMock;
        private readonly Mock<IMotorcycleRepository> _motorcycleRepositoryMock;
        private readonly Mock<IRentalPlanRepository> _rentalPlanRepositoryMock;
        private readonly CreateRentalUseCase _createRentalUseCase;

        public CreateRentalUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<CreateRentalUseCase>>();
            _courierRepositoryMock = new Mock<ICourierRepository>();
            _rentalRepositoryMock = new Mock<IRentalRepository>();
            _motorcycleRepositoryMock = new Mock<IMotorcycleRepository>();
            _rentalPlanRepositoryMock = new Mock<IRentalPlanRepository>();

            _createRentalUseCase = new CreateRentalUseCase(
                _loggerMock.Object,
                _courierRepositoryMock.Object,
                _rentalRepositoryMock.Object,
                _motorcycleRepositoryMock.Object,
                _rentalPlanRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenCourierDoesNotExist()
        {
            // Arrange
            var request = new RentalRequest { CourierIdentifier = "invalid-courier" };
            _courierRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Courier)null);

            // Act
            var result = await _createRentalUseCase.ExecuteAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenMotorcycleDoesNotExist()
        {
            // Arrange
            var request = new RentalRequest { CourierIdentifier = "valid-courier", MotorcycleIdentifier = "invalid-motorcycle" };
            _courierRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Courier());
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Motorcycle)null);

            // Act
            var result = await _createRentalUseCase.ExecuteAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenRentalPlanIsInvalid()
        {
            // Arrange
            var request = new RentalRequest { CourierIdentifier = "valid-courier", MotorcycleIdentifier = "valid-motorcycle", PlanDays = 5 };
            _courierRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Courier());
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Motorcycle());
            _rentalPlanRepositoryMock.Setup(repo => repo.GetByPlanDays(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RentalPlan)null);

            // Act
            var result = await _createRentalUseCase.ExecuteAsync(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidRentalDate_ShouldReturnFalse_WhenStartDateIsInvalid()
        {
            // Arrange
            var request = new RentalRequest { StartDate = DateTime.UtcNow.Date };

            // Act
            var result = _createRentalUseCase.IsValidRentalDate(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidRentalDate_ShouldReturnFalse_WhenEndDateIsInvalid()
        {
            // Arrange
            var request = new RentalRequest { StartDate = DateTime.UtcNow.Date.AddDays(2), EndDate = DateTime.UtcNow.Date.AddDays(1) };

            // Act
            var result = _createRentalUseCase.IsValidRentalDate(request);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValidRentalDate_ShouldReturnTrue_WhenDatesAreValid()
        {
            // Arrange
            var request = new RentalRequest
            {
                StartDate = DateTime.UtcNow.Date.AddDays(2),
                EndDate = DateTime.UtcNow.Date.AddDays(3),
                ExpectedEndDate = DateTime.UtcNow.Date.AddDays(4)
            };

            // Act
            var result = _createRentalUseCase.IsValidRentalDate(request);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrue_WhenDataIsValid()
        {
            // Arrange
            var request = new RentalRequest
            {
                CourierIdentifier = "valid-courier",
                MotorcycleIdentifier = "valid-motorcycle",
                PlanDays = 7,
                StartDate = DateTime.UtcNow.Date.AddDays(1),
                EndDate = DateTime.UtcNow.Date.AddDays(7),
                ExpectedEndDate = DateTime.UtcNow.Date.AddDays(7)
            };
            _courierRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Courier());
            _motorcycleRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Motorcycle());
            _rentalPlanRepositoryMock.Setup(repo => repo.GetByPlanDays(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RentalPlan());
            _rentalRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Rental>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _createRentalUseCase.ExecuteAsync(request);

            // Assert
            Assert.True(result);
        }
    }
}


