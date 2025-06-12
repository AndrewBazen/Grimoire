using Grimoire.Domain.Models;
using Grimoire.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace Grimoire.Domain.UseCases;

public interface ICreateNewTomeUseCase
{
    Task<CreateNewTomeResponse> ExecuteAsync(CreateNewTomeRequest request, CancellationToken ct = default);
}

public sealed record CreateNewTomeRequest(string Name);

public sealed record CreateNewTomeResponse(Tome Tome);

public sealed class CreateNewTomeUseCase : ICreateNewTomeUseCase
{
    private readonly IArchive _archive;
    private readonly ILogger<CreateNewTomeUseCase> _logger;

    public CreateNewTomeUseCase(IArchive archive, ILogger<CreateNewTomeUseCase> logger)
    {
        _archive = archive;
        _logger = logger;
    }

    public async Task<CreateNewTomeResponse> ExecuteAsync(CreateNewTomeRequest request, CancellationToken ct = default)
    {
        var tome = new Tome(Guid.NewGuid().ToString(), request.Name, Environment.UserName, [], DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        await _archive.SaveTomeAsync(tome, ct);
        return new CreateNewTomeResponse(tome);
    }
}