using LawFirm.Domain.Enums;

namespace LawFirm.Domain.Entities;

public class Client
{
    // Private constructor forces every Client to be built through CreateIndividual/CreateOrganization below.
    // This protects STRUCTURE only (ClientType always matches which name fields got set, can't be reassigned
    // later since ClientType has a private setter) - it does NOT validate values. A caller passing an
    // out-of-range enum (e.g. (ClientStatus)99) still gets happily accepted here; that check is a separate
    // job done by ClientService.ValidateCreate before this factory method is ever called.
    private Client() { }

    public int Id { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

    public string ClientCode { get; private set; } = string.Empty;

    public ClientType ClientType { get; private set; }
    public ClientStatus Status { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PreferredName { get; set; }
    public DateOnly? DateOfBirth { get; set; }

    public string? OrganizationName { get; set; }
    public string? TradingName { get; set; }
    public string? CompanyNumber { get; set; }

    public string? Email { get; set; }
    public string? Phone { get; set; }

    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Postcode { get; set; }
    public string? Country { get; set; }

    public string? InternalNotesSummary { get; set; }

    public bool IsArchived { get; private set; }

    private readonly List<ClientContact> _contacts = [];
    public List<ClientContact> Contacts => _contacts.ToList();

    public string DisplayName => ClientType == ClientType.Organization
        ? OrganizationName ?? string.Empty
        : $"{FirstName} {LastName}".Trim();

    public static Client CreateIndividual(string clientCode, string firstName, string lastName, ClientStatus status)
    {
        var client = new Client
        {
            ClientCode = clientCode,
            ClientType = ClientType.Individual,
            Status = status,
            FirstName = firstName,
            LastName = lastName
        };
        return client;
    }

    public static Client CreateOrganization(string clientCode, string organizationName, ClientStatus status)
    {
        var client = new Client
        {
            ClientCode = clientCode,
            ClientType = ClientType.Organization,
            Status = status,
            OrganizationName = organizationName
        };
        return client;
    }

    public ClientContact AddContact(string name, string relationshipType, string? email, string? phone, string? company, string? notes)
    {
        var contact = ClientContact.Create(Id, name, relationshipType, email, phone, company, notes);
        _contacts.Add(contact);
        return contact;
    }

    public void Archive() => IsArchived = true;
    public void Restore() => IsArchived = false;
}
