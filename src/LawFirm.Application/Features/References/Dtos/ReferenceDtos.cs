using LawFirm.Domain.Enums;

namespace LawFirm.Application.Features.References.Dtos;

public class ReferenceListRequest
{
    public string? Keyword { get; set; }

    public ReferenceDataType ReferenceDataType { get; set; }

    public bool IncludeHidden { get; set; }
}

public record ReferenceListItemDto(
    int Id,
    string? ReferenceCode,
    ReferenceDataType ReferenceDataType,
    string Name,
    DateTimeOffset CreatedAt,
    int? DisplayOrder,
    bool IsActive
);

public record ReferenceDetailDto(
    int Id,
    int? DisplayOrder,
    bool IsActive,
    ReferenceDataType ReferenceDataType,
    string? ReferenceCode,
    string CreatedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt,
    string? UpdatedBy,
    string Name
);

public record CreateReferenceRequest(
    string Name,
    int? DisplayOrder,
    ReferenceDataType ReferenceDataType,
    string? ReferenceCode
);

public record UpdateReferenceRequest(
    string Name,
    int? DisplayOrder,
    string? ReferenceCode
);
