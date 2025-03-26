using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using Xunit;
using MotorCycleRentail.Domain.Entities;

namespace MotorCycleRentail.Test.Unit.Application.Usecases.Rentals
{
    public class ReturnMotocycleRentalUseCaseTest
    {
        private readonly Mock<ILogger<ReturnMotocycleRentalUsecase>> _loggerMock;
        private readonly Mock<IRentalRepository> _rentalRepositoryMock;
        private readonly ReturnMotocycleRentalUsecase _returnMotocycleRentalUsecase;

        public ReturnMotocycleRentalUseCaseTest()
        {
            _loggerMock = new Mock<ILogger<ReturnMotocycleRentalUsecase>>();
            _rentalRepositoryMock = new Mock<IRentalRepository>();

            _returnMotocycleRentalUsecase = new ReturnMotocycleRentalUsecase(
                _loggerMock.Object,
                _rentalRepositoryMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenIdIsInvalid()
        {
            // Arrange
            string id = "invalid-guid";
            DateTime newReturnDate = DateTime.UtcNow;

            // Act
            var result = await _returnMotocycleRentalUsecase.ExecuteAsync(id, newReturnDate, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenRentalDoesNotExist()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            DateTime newReturnDate = DateTime.UtcNow;
            _rentalRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Rental)null);

            // Act
            var result = await _returnMotocycleRentalUsecase.ExecuteAsync(id, newReturnDate, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenNewReturnDateIsBeforeStartDate()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            DateTime newReturnDate = DateTime.UtcNow.AddDays(-1);
            var rental = new Rental { Id = Guid.Parse(id), StartDate = DateTime.UtcNow };
            _rentalRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rental);

            // Act
            var result = await _returnMotocycleRentalUsecase.ExecuteAsync(id, newReturnDate, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrue_WhenDataIsValid()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            DateTime newReturnDate = DateTime.UtcNow.AddDays(1);
            var rental = new Rental { Id = Guid.Parse(id), StartDate = DateTime.UtcNow };
            _rentalRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(rental);
            _rentalRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Rental>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _returnMotocycleRentalUsecase.ExecuteAsync(id, newReturnDate, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}



