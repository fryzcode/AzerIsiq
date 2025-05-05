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
    
    [Fact]
    public async Task CreateSubscriberRequestAsync_Throws_When_Fin_Requests_Exceeded()
    {
        var dto = new SubscriberRequestDto { FinCode = "ABC123", Building = "B", Apartment = "1" };
        _authServiceMock.Setup(x => x.GetCurrentUserId()).Returns(1);

        var requests = new List<Subscriber> {
            new Subscriber { CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Subscriber { CreatedAt = DateTime.UtcNow.AddDays(-5) },
            new Subscriber { CreatedAt = DateTime.UtcNow.AddDays(-1) }
        };

        _subscriberRepositoryMock.Setup(x => x.GetRequestsByFinAsync(dto.FinCode)).ReturnsAsync(requests);

        await Assert.ThrowsAsync<Exception>(() => _service.CreateSubscriberRequestAsync(dto));
    }
    
    [Fact]
    public async Task CreateSubscriberCodeAsync_Throws_When_Subscriber_NotFound()
    {
        _subscriberRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Subscriber)null);
        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateSubscriberCodeAsync(1));
    }
    
    [Fact]
    public async Task CreateCounterForSubscriberAsync_Returns_Subscriber()
    {
        var subscriber = new Subscriber { Id = 1, Status = 0 };
        var dto = new CounterDto();

        _subscriberRepositoryMock.Setup(x => x.GetByIdAsync(subscriber.Id)).ReturnsAsync(subscriber);
        _counterServiceMock.Setup(x => x.CreateCountersAsync(dto, subscriber.Id)).ReturnsAsync(new Counter());
        _subscriberRepositoryMock.Setup(x => x.UpdateAsync(subscriber)).Returns(Task.CompletedTask);

        var result = await _service.CreateCounterForSubscriberAsync(subscriber.Id, dto);
        Assert.Equal(subscriber.Id, result.Id);
    }
    
    [Fact]
    public async Task ConnectTmToSubscriberAsync_Updates_TmId()
    {
        var subscriber = new Subscriber { Id = 1, Status = 0 };
        _subscriberRepositoryMock.Setup(r => r.GetByIdAsync(subscriber.Id)).ReturnsAsync(subscriber);
        _tmServiceMock.Setup(s => s.GetTmByIdAsync(2)).ReturnsAsync(new TmGetDto());
        _subscriberRepositoryMock.Setup(r => r.UpdateAsync(subscriber)).Returns(Task.CompletedTask);

        var result = await _service.ConnectTmToSubscriberAsync(subscriber.Id, 2);
        Assert.Equal(2, result.TmId);
    }
    
    [Fact]
    public async Task ApplySubscriberContractAsync_Sets_Status_When_Eligible()
    {
        var subscriber = new Subscriber { Id = 1, Status = (int)SubscriberStatus.TmConnected };
        _subscriberRepositoryMock.Setup(x => x.GetByIdAsync(subscriber.Id)).ReturnsAsync(subscriber);
        _subscriberRepositoryMock.Setup(x => x.UpdateAsync(subscriber)).Returns(Task.CompletedTask);

        var (confirmed, result) = await _service.ApplySubscriberContractAsync(subscriber.Id);

        Assert.False(confirmed);
        Assert.Equal((int)SubscriberStatus.ContractSigned, result.Status);
    }
}