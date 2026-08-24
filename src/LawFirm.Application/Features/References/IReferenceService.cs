using LawFirm.Application.Features.References.Dtos;
using LawFirm.Shared.Results;

namespace LawFirm.Application.Features.References;

public interface IReferenceService
{
    Task<List<ReferenceListItemDto>> ListAsync(ReferenceListRequest request);

    Task<Result<ReferenceDetailDto>> GetByIdAsync(int id);

    Task<Result<ReferenceDetailDto>> CreateAsync(CreateReferenceRequest request);

    Task<Result<ReferenceDetailDto>> UpdateAsync(int id, UpdateReferenceRequest request);

    Task<Result> ShowAsync(int id);

    Task<Result> HideAsync(int id);
}
