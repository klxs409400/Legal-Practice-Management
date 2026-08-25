using System.ComponentModel.DataAnnotations;
using LawFirm.Application.Features.References;
using LawFirm.Application.Features.References.Dtos;
using LawFirm.Domain.Entities;
using LawFirm.Domain.Enums;
using LawFirm.Infrastructure.Persistence;
using LawFirm.Shared.Results;
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

        var reference = Reference.CreateSpecificCategoryName(request.Name, "System", request.ReferenceDataType, request.DisplayOrder);

        reference.ApplyCommonFields(request.ReferenceCode, request.Name, request.DisplayOrder);

        _db.References.Add(reference);
        await _db.SaveChangesAsync();

        return reference.ToDetail();
    }

    public async Task<Result<ReferenceDetailDto>> UpdateAsync(int id, UpdateReferenceRequest request)
    {
        var reference = await _db.References
        .FirstOrDefaultAsync(r => r.Id == id);

        if (reference is null)
            return Error.NotFound("Reference", id);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Error.Validation("Name is required for a Reference.");
        }

        reference.ApplyCommonFields(request.ReferenceCode, request.Name, request.DisplayOrder);
        reference.UpdatedAt = DateTimeOffset.UtcNow;
        reference.UpdatedBy = "System";

        await _db.SaveChangesAsync();
        return reference.ToDetail();

    }

    public async Task<Result> ShowAsync(int id)
    {
        var reference = await _db.References
        .FirstOrDefaultAsync(r => r.Id == id);

        if (reference is null)
            return Result.Failure(Error.NotFound("Reference", id));


        reference.ShowReference();
        await _db.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> HideAsync(int id)
    {
        var reference = await _db.References
       .FirstOrDefaultAsync(r => r.Id == id);

        if (reference is null)
            return Result.Failure(Error.NotFound("Reference", id));

        reference.HideReference();
        await _db.SaveChangesAsync();
        return Result.Success();
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