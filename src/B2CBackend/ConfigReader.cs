namespace B2CBackend
{
    public class ConfigReader
    {
        public string TenantId { get; internal set; }
        public string ClientId { get; internal set; }
        public string ClientSecret { get; internal set; }

        public ConfigReader()
        {
            TenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID") ?? string.Empty;
            ClientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID") ?? string.Empty;
            ClientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET") ?? string.Empty;
        }
    }
}
