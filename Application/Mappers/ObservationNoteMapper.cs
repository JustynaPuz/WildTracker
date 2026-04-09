using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Entities;

namespace WildTracker.Application.Mappers;

public static class ObservationNoteMapper
{
    public static ObservationNoteDto ToDto(ObservationNote entity) => new()
    {
        Id = entity.Id,
        SightingReportId = entity.SightingReportId,
        AuthorUserId = entity.AuthorUserId,
        Content = entity.Content,
        CreatedAtUtc = entity.CreatedAtUtc
    };
}
