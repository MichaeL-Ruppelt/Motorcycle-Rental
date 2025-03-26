using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using Xunit;
using MotorCycleRentail.Domain.Entities;

namespace MotorCycleRentail.Test.Unit.Application.Usecases.Rentals
{
    public class CalculateRentalPriceUseCaseTest
    {
        private readonly Mock<ILogger<CalculateRentalPriceUseCase>> _loggerMock;
        private readonly Mock<IRentalRepository> _rentalRepositoryMock;
        private readonly Mock<IRentalPlanRepository> _rentalPlanRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly CalculateRentalPriceUseCase _calculateRentalPriceUseCase;

        public CalculateRentalPriceUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<CalculateRentalPriceUseCase>>();
            _rentalRepositoryMock = new Mock<IRentalRepository>();
            _rentalPlanRepositoryMock = new Mock<IRentalPlanRepository>();
            _configurationMock = new Mock<IConfiguration>();

            _configurationMock.Setup(config => config["BusinessRules:OvertakeFineValue"]).Returns("10");

            _calculateRentalPriceUseCase = new CalculateRentalPriceUseCase(
                _loggerMock.Object,
                _rentalRepositoryMock.Object,
                _rentalPlanRepositoryMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenRentalDoesNotExist()
        {
            // Arrange
            string id = "rental-id";
            _rentalRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rental)null);

            // Act
            var result = await _calculateRentalPriceUseCase.ExecuteAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNull_WhenRentalPlanDoesNotExist()
        {
            // Arrange
            string id = "rental-id";
            var rental = new Rental { PlanDays = 5 };
            _rentalRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rental);
            _rentalPlanRepositoryMock.Setup(repo => repo.GetByPlanDays(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((RentalPlan)null);

            // Act
            var result = await _calculateRentalPriceUseCase.ExecuteAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnRentalResponse_WhenDataIsValid()
        {
            // Arrange
            var rental = new Rental
            {
                PlanDays = 7,
                CourierIdentifier = "courier1",
                MotorcycleIdentifier = "motorcycle1",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                ExpectedEndDate = DateTime.UtcNow.AddDays(7)
            };

            string rentalId = rental.Id.ToString();

            var rentalPlan = new RentalPlan { PlanValue = 20, FineValue = 10 };
            _rentalRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rental);
            _rentalPlanRepositoryMock.Setup(repo => repo.GetByPlanDays(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rentalPlan);

            // Act
            var result = await _calculateRentalPriceUseCase.ExecuteAsync(rentalId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rental.Id.ToString(), result.Identifier);
        }
    }
}


