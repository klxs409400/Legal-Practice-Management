using LawFirm.Application.Features.References.Dtos;
using LawFirm.Domain.Entities;

namespace LawFirm.Application.Features.References;

public static class ReferenceMappings
{
    public static ReferenceDetailDto ToDetail(this Reference r) => new(
        r.Id,
        r.DisplayOrder,
        r.IsActive,
        r.ReferenceDataType,
        r.ReferenceCode,
        r.CreatedBy,
        r.CreatedAt,
        r.UpdatedAt,
        r.UpdatedBy,
        r.Name
    );

    public static ReferenceListItemDto ToListItem(this Reference r) => new(
        r.Id,
        r.ReferenceCode,
        r.ReferenceDataType,
        r.Name,
        r.CreatedAt,
        r.DisplayOrder,
        r.IsActive
    );

    public static void ApplyCommonFields(this Reference r, string? referenceCode, string name, int? displayOrder)
    {
        r.ReferenceCode = referenceCode;
        r.Name = name;
        r.DisplayOrder = displayOrder;
    }

}




