using Microsoft.EntityFrameworkCore;
using Refuel.Domain.Entities;
using Refuel.Persistence;
using Refuel.Persistence.Repositories;

namespace Refuel.Tests.Persistence;

public class RepositoryTests : IDisposable
{
    private readonly RefuelDbContext _context;
    private readonly Repository<GasStation> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<RefuelDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new RefuelDbContext(options);
        _repository = new Repository<GasStation>(_context);
    }

    [Fact]
    public async Task AddAsync_ThenGetById_ReturnsEntity()
    {
        var station = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);
        await _repository.AddAsync(station);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(station.Id);

        Assert.NotNull(result);
        Assert.Equal("Shell", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        await _repository.AddAsync(new GasStation("Shell", "Via Roma 1", 45.0, 11.0));
        await _repository.AddAsync(new GasStation("Eni", "Corso Italia 5", 46.0, 12.0));
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Delete_RemovesEntity()
    {
        var station = new GasStation("Shell", "Via Roma 1", 45.0, 11.0);
        await _repository.AddAsync(station);
        await _context.SaveChangesAsync();

        _repository.Delete(station);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(station.Id);
        Assert.Null(result);
    }

    public void Dispose() => _context.Dispose();
}