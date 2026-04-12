using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;

namespace WildTracker.Application.Services;

public class ObservationNoteService : IObservationNoteService
{
    private readonly IObservationNoteRepository _repo;
    private readonly ISightingReportRepository _reportRepo;

    public ObservationNoteService(IObservationNoteRepository repo, ISightingReportRepository reportRepo)
    {
        _repo = repo;
        _reportRepo = reportRepo;
    }

    public async Task<IEnumerable<ObservationNoteDto>> GetByReportIdAsync(Guid reportId)
    {
        var notes = await _repo.GetByReportIdAsync(reportId);
        return notes.Select(ObservationNoteMapper.ToDto);
    }

    public async Task<ObservationNoteDto> CreateAsync(Guid reportId, string content, Guid authorUserId)
    {
        var reportExists = await _reportRepo.GetByIdAsync(reportId) is not null;
        if (!reportExists)
            throw new NotFoundException($"Sighting report with id '{reportId}' was not found.");

        var entity = new ObservationNote(reportId, authorUserId, content);
        await _repo.AddAsync(entity);
        return ObservationNoteMapper.ToDto(entity);
    }
}
