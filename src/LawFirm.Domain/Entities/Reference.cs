using LawFirm.Domain.Enums;

namespace LawFirm.Domain.Entities;

public class Reference
{
    private Reference()
    {

    }
    public int Id { get; private set; }
    public int? DisplayOrder { get; set; }
    public bool IsActive { get; private set; } = true;
    public ReferenceDataType ReferenceDataType { get; private set; }
    public string? ReferenceCode { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public string Name { get; set; } = string.Empty;


    public static Reference CreateSpecificCategoryName(string name, string createBy, ReferenceDataType referenceDataType, int? displayOrder)
    {
        var newReference = new Reference
        {
            Name = name,
            CreatedBy = createBy,
            CreatedAt = DateTimeOffset.Now,
            ReferenceDataType = referenceDataType,
            DisplayOrder = displayOrder
        };
        return newReference;
    }

    public void HideReference() => IsActive = false;

    public void ShowReference() => IsActive = true;


}