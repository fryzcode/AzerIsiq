using AutoMapper;
using AzerIsiq.Dtos;
using AzerIsiq.Extensions.Enum;
using AzerIsiq.Extensions.Exceptions;
using AzerIsiq.Models;
using AzerIsiq.Repository.Interface;
using AzerIsiq.Services;
using AzerIsiq.Services.Helpers;
using AzerIsiq.Services.ILogic;
using Moq;

namespace AzerIsiq.Tests;
using Xunit;

public class SubscriberServiceTests
{
    private readonly Mock<ISubscriberRepository> _subscriberRepositoryMock = new();
    private readonly Mock<ICounterService> _counterServiceMock = new();
    private readonly Mock<ITmService> _tmServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ISubscriberCodeGenerator> _codeGeneratorMock = new();
    private readonly Mock<ILoggingService> _loggingServiceMock = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<ICounterRepository> _counterRepositoryMock = new();

    private readonly SubscriberService _service;

    public SubscriberServiceTests()
    {
        _authServiceMock.Setup(x => x.GetCurrentUserId()).Returns(1);

        _service = new SubscriberService(
            _subscriberRepositoryMock.Object,
            _counterServiceMock.Object,
            _tmServiceMock.Object,
            _mapperMock.Object,
            _codeGeneratorMock.Object,
            _loggingServiceMock.Object,
            _authServiceMock.Object,
            _counterRepositoryMock.Object
        );
    }

    [Fact]
    public async Task CreateSubscriberRequestAsync_ShouldThrowException_WhenFinRequestLimitExceeded()
    {
        // Arrange
        var dto = new SubscriberRequestDto() { FinCode = "1234567890" };

        var oldRequest = new Subscriber() { CreatedAt = DateTime.UtcNow.AddDays(-10) };
        _subscriberRepositoryMock
            .Setup(x => x.GetRequestsByFinAsync(dto.FinCode))
            .ReturnsAsync(new List<Subscriber> { oldRequest, oldRequest, oldRequest });

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() =>
            _service.CreateSubscriberRequestAsync(dto));
        
        Assert.Equal("Requests with FIN 1234567890 exceed the limit", ex.Message);
    }
    

}