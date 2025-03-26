using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MotorCycleRentail.Application.Services;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.Entities;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using Xunit;

namespace MotorCycleRentail.Test.Unit;

public class CreateCourierUsecaseTests
{
    private readonly Mock<ILogger<CreateCourierUsecase>> _loggerMock;
    private readonly Mock<ICourierRepository> _courierRepositoryMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly CreateCourierUsecase _createCourierUsecase;

    public CreateCourierUsecaseTests()
    {
        _loggerMock = new Mock<ILogger<CreateCourierUsecase>>();
        _courierRepositoryMock = new Mock<ICourierRepository>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();

        _createCourierUsecase = new CreateCourierUsecase(
            _loggerMock.Object,
            _courierRepositoryMock.Object,
            _fileStorageServiceMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalse_WhenCnhImageIsInvalid()
    {
        // Arrange
        var request = new CourierRequest { CnhImage = "invalid-base64" };

        // Act
        var result = await _createCourierUsecase.ExecuteAsync(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalse_WhenCourierAlreadyExists()
    {
        // Arrange
        var request = new CourierRequest { CnhImage = "valid-base64", CnhNumber = "123456", Cnpj = "123456789" };
        _courierRepositoryMock.Setup(repo => repo.CheckIfCourierAlreadyExist(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _createCourierUsecase.ExecuteAsync(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnFalse_WhenDataIsInvalid()
    {
        // Arrange
        var request = new CourierRequest { CnhImage = "valid-base64", CnhNumber = "123456", Cnpj = "123456789" };
        _courierRepositoryMock.Setup(repo => repo.CheckIfCourierAlreadyExist(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _fileStorageServiceMock.Setup(service => service.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("new-image-id");

        // Act
        var result = await _createCourierUsecase.ExecuteAsync(request);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnTrue_WhenDataIsValid()
    {
        // Arrange
        var request = new CourierRequest
        {
            Identifier = "identifier",
            Name = "name",
            Cnpj = "12345678911111",
            Birthdate = DateTime.UtcNow,
            CnhNumber = "12345678911",
            CnhImage = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=",
            CnhType = "A"
        };
        _courierRepositoryMock.Setup(repo => repo.CheckIfCourierAlreadyExist(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _fileStorageServiceMock.Setup(service => service.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync("new-image-id");

        _courierRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<Courier>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);


        // Act
        var result = await _createCourierUsecase.ExecuteAsync(request);

        // Assert
        Assert.True(result);
    }
}
