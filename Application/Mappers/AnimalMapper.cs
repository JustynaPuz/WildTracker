using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;
using WildTracker.Domain.Entities;

namespace WildTracker.Application.Mappers;

public static class AnimalMapper
{
    public static AnimalDto ToDto(Animal entity) => new()
    {
        Id = entity.Id,
        Identifier = entity.Identifier,
        Name = entity.Name,
        Species = entity.Species,
        HealthStatus = entity.HealthStatus,
        Description = entity.Description,
        LastSeenAtUtc = entity.LastSeenAtUtc,
        CreatedAtUtc = entity.CreatedAtUtc
    };

    public static Animal ToEntity(CreateAnimalRequest request) => new(
        request.Identifier,
        request.Name,
        request.Species,
        request.HealthStatus,
        request.Description
    );
}
