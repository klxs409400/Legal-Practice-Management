using LawFirm.Application.Features.Clients;
using LawFirm.Application.Features.Clients.Dtos;
using LawFirm.Domain.Enums;
using LawFirm.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LawFirm.Application.Tests;

public class ClientServiceTests
{
    // Each test gets its own isolated in-memory database (unique name per test run) so tests
    // never see each other's data - InMemory databases are keyed by name, shared across
    // DbContext instances that use the same name.
    private static LawFirmDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<LawFirmDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LawFirmDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ValidIndividual_Succeeds()
    {
        await using var db = CreateDbContext();
        var service = new ClientService(db);
        var request = new CreateClientRequest(
            ClientType.Individual, ClientStatus.ActiveClient,
            "Jane", "Doe", null, null,
            null, null, null,
            "jane.doe@example.com", null,
            null, null);

        var result = await service.CreateAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(ClientType.Individual, result.Value!.ClientType);
        Assert.StartsWith("CLI-", result.Value.ClientCode);
    }
}
