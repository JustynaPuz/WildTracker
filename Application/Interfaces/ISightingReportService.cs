using WildTracker.Contracts.Common;
using WildTracker.Contracts.DTOs;
using WildTracker.Contracts.Requests;

namespace WildTracker.Application.Interfaces;

public interface ISightingReportService
{
    Task<SightingReportDto> GetByIdAsync(Guid id);
    Task<PagedResult<SightingReportDto>> SearchAsync(SightingReportSearchRequest request);
    Task<SightingReportDto> CreateAsync(CreateSightingReportRequest request, Guid reportedByUserId);
    Task<SightingReportDto> UpdateAsync(Guid id, UpdateSightingReportRequest request);
    Task<SightingReportDto> ApproveAsync(Guid id);
    Task<SightingReportDto> RejectAsync(Guid id);
    Task<SightingReportDto> ResolveAsync(Guid id);
    Task DeleteAsync(Guid id);
}
