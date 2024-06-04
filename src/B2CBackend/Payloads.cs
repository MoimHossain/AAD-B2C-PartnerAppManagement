using System.Text.Json.Serialization;

namespace B2CBackend
{
    public record GeneratePayload(string DisplayName);

    public record AppReg(
        string? ClientId,
        string? SecretId,
        string? DisplayName,
        string? ClientSecret,
        string? TenantId,
        DateTimeOffset? StartDateTime,
        DateTimeOffset? EndDateTime);



    public record Identity(
        [property: JsonPropertyName("provider")] string Provider,
        [property: JsonPropertyName("id")] string Id
    );

    public record ApiUserProperties(
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("state")] string State,
        [property: JsonPropertyName("registrationDate")] DateTime RegistrationDate,
        [property: JsonPropertyName("note")] object Note,
        [property: JsonPropertyName("identities")] IReadOnlyList<Identity> Identities
    );

    public record ApiUserDto(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("properties")] ApiUserProperties Properties
    );


}
