using WildTracker.Application.Exceptions;
using WildTracker.Application.Interfaces;
using WildTracker.Application.Mappers;
using WildTracker.Contracts.DTOs;
using WildTracker.Domain.Entities;
using WildTracker.Domain.Repositories;

namespace WildTracker.Application.Services;

public class ObservationNoteService : IObservationNoteService
{
    private readonly IObservationNoteRepository _noteRepository;
    private readonly ISightingReportRepository _reportRepository;

    public ObservationNoteService(IObservationNoteRepository noteRepository, ISightingReportRepository reportRepository)
    {
        _noteRepository = noteRepository;
        _reportRepository = reportRepository;
    }

    public async Task<IReadOnlyList<ObservationNoteDto>> GetByReportIdAsync(Guid reportId)
    {
        _ = await _reportRepository.GetByIdAsync(reportId)
            ?? throw new NotFoundException($"Sighting report with id '{reportId}' was not found.");

        var notes = await _noteRepository.GetByReportIdAsync(reportId);
        return notes.Select(ObservationNoteMapper.ToDto).ToList();
    }

    public async Task<ObservationNoteDto> CreateAsync(Guid reportId, string content, Guid authorUserId)
    {
        _ = await _reportRepository.GetByIdAsync(reportId)
            ?? throw new NotFoundException($"Sighting report with id '{reportId}' was not found.");

        var entity = new ObservationNote(reportId, authorUserId, content);
        await _noteRepository.AddAsync(entity);
        return ObservationNoteMapper.ToDto(entity);
    }

    public async Task<ObservationNoteDto> UpdateAsync(Guid reportId, Guid noteId, string content)
    {
        var note = await _noteRepository.GetByIdAsync(noteId)
            ?? throw new NotFoundException($"Observation note with id '{noteId}' was not found.");

        if (note.SightingReportId != reportId)
            throw new NotFoundException($"Observation note with id '{noteId}' was not found.");

        note.UpdateContent(content);
        await _noteRepository.UpdateAsync(note);
        return ObservationNoteMapper.ToDto(note);
    }

    public async Task DeleteAsync(Guid reportId, Guid noteId)
    {
        var note = await _noteRepository.GetByIdAsync(noteId)
            ?? throw new NotFoundException($"Observation note with id '{noteId}' was not found.");

        if (note.SightingReportId != reportId)
            throw new NotFoundException($"Observation note with id '{noteId}' was not found.");

        await _noteRepository.DeleteAsync(note);
    }
}
