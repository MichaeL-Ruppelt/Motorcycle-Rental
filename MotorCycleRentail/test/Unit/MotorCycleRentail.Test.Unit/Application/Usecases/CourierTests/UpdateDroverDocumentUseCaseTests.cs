using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MotorCycleRentail.Application.Services;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Common.Helpers;
using MotorCycleRentail.Domain.Entities;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using Xunit;

namespace MotorCycleRentail.Test.Unit
{
    public class UpdateDriverDocumentUseCaseTests
    {
        private readonly Mock<ILogger<UpdateDriverDocumentUseCase>> _loggerMock;
        private readonly Mock<ICourierRepository> _courierRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly UpdateDriverDocumentUseCase _updateDriverDocumentUseCase;

        public UpdateDriverDocumentUseCaseTests()
        {
            _loggerMock = new Mock<ILogger<UpdateDriverDocumentUseCase>>();
            _courierRepositoryMock = new Mock<ICourierRepository>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();

            _updateDriverDocumentUseCase = new UpdateDriverDocumentUseCase(
                _loggerMock.Object,
                _courierRepositoryMock.Object,
                _fileStorageServiceMock.Object
            );
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenCnhImageIsInvalid()
        {
            // Arrange
            var courierIdentifier = "courier-id-test";
            var newCnhImage = "invalid-base64";

            // Act
            var result = await _updateDriverDocumentUseCase.ExecuteAsync(courierIdentifier, newCnhImage);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnFalse_WhenCourierDoesNotExist()
        {
            // Arrange
            var courierIdentifier = "courier-id-test";
            var newCnhImage = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";
            _courierRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Courier)null);

            // Act
            var result = await _updateDriverDocumentUseCase.ExecuteAsync(courierIdentifier, newCnhImage);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnTrue_WhenDataIsValid()
        {
            // Arrange
            var courierIdentifier = "courier-id-test";
            var newCnhImage = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";
            var courier = new Courier { Id = Guid.NewGuid(), CnhImageId = "old-image-id" };
            _courierRepositoryMock.Setup(repo => repo.GetByIdentifierAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(courier);
            _fileStorageServiceMock.Setup(service => service.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("new-image-id");
            _courierRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Courier>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            _fileStorageServiceMock.Setup(service => service.DeleteFile(It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = await _updateDriverDocumentUseCase.ExecuteAsync(courierIdentifier, newCnhImage);

            // Assert
            Assert.True(result);
        }
    }
}
