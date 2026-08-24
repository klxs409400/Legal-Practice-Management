using LawFirm.Domain.Enums;

namespace LawFirm.Application.Features.Clients.Dtos;

// A plain class (not a positional record like CreateClientRequest below) because this is
// bound via [FromQuery] on ClientsController.List, not [FromBody]. Query-string binding
// builds a parameterless instance first, then sets only the properties that showed up in
// the URL one at a time - that needs a real parameterless constructor + settable props,
// which a positional record's "all args at once" constructor can't provide. [FromBody]
// JSON, by contrast, arrives as one complete object, so a positional record works fine there.
public class ClientListRequest
{
    public string? Keyword { get; set; }
    public ClientType? ClientType { get; set; }
    public ClientStatus? Status { get; set; }

    public bool IncludeArchived { get; set; }
}

public record ClientListItemDto(
    int Id,
    string ClientCode,
    string DisplayName,
    ClientType ClientType,
    string? Email,
    string? Phone,
    ClientStatus Status,
    DateTimeOffset CreatedAt);

public record ClientDetailDto(
    int Id,
    string ClientCode,
    string DisplayName,
    ClientType ClientType,
    ClientStatus Status,
    string? FirstName,
    string? LastName,
    string? PreferredName,
    DateOnly? DateOfBirth,
    string? OrganizationName,
    string? TradingName,
    string? CompanyNumber,
    string? Email,
    string? Phone,
    AddressDto Address,
    string? InternalNotesSummary,
    bool IsArchived,
    DateTimeOffset CreatedAt,
    string CreatedBy,
    DateTimeOffset? UpdatedAt,
    string? UpdatedBy,
    List<ClientContactDto> Contacts);

public record AddressDto(
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? State,
    string? Postcode,
    string? Country);

public record ClientContactDto(
    int Id,
    string Name,
    string RelationshipType,
    string? Email,
    string? Phone,
    string? Company,
    string? Notes,
    bool IsActive);

public record CreateClientRequest(
    ClientType ClientType,
    ClientStatus Status,
    string? FirstName,
    string? LastName,
    string? PreferredName,
    DateOnly? DateOfBirth,
    string? OrganizationName,
    string? TradingName,
    string? CompanyNumber,
    string? Email,
    string? Phone,
    AddressDto? Address,
    string? InternalNotesSummary);

public record UpdateClientRequest(
    ClientStatus Status,
    string? FirstName,
    string? LastName,
    string? PreferredName,
    DateOnly? DateOfBirth,
    string? OrganizationName,
    string? TradingName,
    string? CompanyNumber,
    string? Email,
    string? Phone,
    AddressDto? Address,
    string? InternalNotesSummary);

public record CreateClientContactRequest(
    string Name,
    string RelationshipType,
    string? Email,
    string? Phone,
    string? Company,
    string? Notes);
