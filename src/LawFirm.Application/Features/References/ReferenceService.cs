using System.ComponentModel.DataAnnotations;
using LawFirm.Application.Features.References;
using LawFirm.Application.Features.References.Dtos;
using LawFirm.Domain.Entities;
using LawFirm.Domain.Enums;
using LawFirm.Infrastructure.Persistence;
using LawFirm.Shared.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LawFirm.Application.Features.References;

public class ReferenceService : IReferenceService
{
    private readonly LawFirmDbContext _db;

    public ReferenceService(LawFirmDbContext db)
    {
        _db = db;
    }

    public async Task<List<ReferenceListItemDto>> ListAsync(ReferenceListRequest request)
    {
        IQueryable<Reference> query = _db.References.AsNoTracking();

        if (!request.IncludeHidden)
        {
            query = query.Where(r => r.IsActive);
        }

        query = query.Where(r => r.ReferenceDataType == request.ReferenceDataType);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keywords = request.Keyword.Trim();
            query = query.Where(r =>
            (r.ReferenceCode != null && r.ReferenceCode.Contains(keywords)) ||
            (r.Name.Contains(keywords)));
        }

        return await query
        .OrderByDescending(r => r.CreatedAt)
        .Select(r => r.ToListItem())
        .ToListAsync();
    }

    public async Task<Result<ReferenceDetailDto>> GetByIdAsync(int id)
    {
        var reference = await _db.References
        .AsNoTracking()
        .FirstOrDefaultAsync(r => r.Id == id);

        return reference is null
        ? Error.NotFound("Reference", id)
        : reference.ToDetail();
    }

    public async Task<Result<ReferenceDetailDto>> CreateAsync(CreateReferenceRequest request)
    {
        if (ValidateCreate(request) is { } error)
        {
            return error;
        }

        var reference = Reference.CreateSpecificCategoryName(request.Name, request.DisplayOrder,)
    }





    private static Error? ValidateCreate(CreateReferenceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Error.Validation("Name is required for a Reference.");
        }

        if (!Enum.IsDefined(request.ReferenceDataType))
        {
            return Error.Validation("Reference data type is required");
        }

        return null;
    }

}