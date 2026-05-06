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
        _repo       = repo;
        _reportRepo = reportRepo;
    }

    public async Task<IEnumerable<ObservationNoteDto>> GetByReportIdAsync(Guid reportId)
    {
        var notes = await _repo.GetByReportIdAsync(reportId);
        return notes.Select(ObservationNoteMapper.ToDto);
    }

    public async Task<ObservationNoteDto> CreateAsync(Guid reportId, string content, Guid authorUserId)
    {
        _ = await _reportRepo.GetByIdAsync(reportId)
            ?? throw new NotFoundException($"Sighting report with id '{reportId}' was not found.");

        var entity = new ObservationNote(reportId, authorUserId, content);
        await _repo.AddAsync(entity);
        return ObservationNoteMapper.ToDto(entity);
    }

    public async Task<ObservationNoteDto> UpdateAsync(Guid reportId, Guid noteId, string content)
    {
        var note = await GetNoteOrThrow(reportId, noteId);
        note.UpdateContent(content);
        await _repo.UpdateAsync(note);
        return ObservationNoteMapper.ToDto(note);
    }

    public async Task DeleteAsync(Guid reportId, Guid noteId)
    {
        var note = await GetNoteOrThrow(reportId, noteId);
        await _repo.DeleteAsync(note);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Fetches a note and validates it belongs to the given report.
    /// Prevents accessing notes from a different report via URL manipulation.
    /// </summary>
    private async Task<ObservationNote> GetNoteOrThrow(Guid reportId, Guid noteId)
    {
        var note = await _repo.GetByIdAsync(noteId)
            ?? throw new NotFoundException($"Note with id '{noteId}' was not found.");

        if (note.SightingReportId != reportId)
            throw new NotFoundException($"Note with id '{noteId}' was not found on report '{reportId}'.");

        return note;
    }
}
