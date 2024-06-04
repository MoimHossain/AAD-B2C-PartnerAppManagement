namespace B2CBackend
{
    public class ApimService(
        ILogger<ApimService> logger,
        IHttpClientFactory httpClientFactory)
    {
        public async Task<ApiUserDto?> GetUserAsync(InvocationContext? context)
        {
            try
            {
                return await GetUserCoreAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting user from APIM");
            }
            return default;
        }

        private async Task<ApiUserDto?> GetUserCoreAsync(InvocationContext? context)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(context.Token);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(context.UserId);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(context.ApiVersion);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(context.Hostname);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(context.ManagementApiUrl);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(context.Origin);

            var client = httpClientFactory.CreateClient();

            var requestPath = $"{context.ManagementApiUrl}/users/{context.UserId}?api-version={context.ApiVersion}";
            client.DefaultRequestHeaders.Add("Authorization", context.Token);
            var response = await client.GetAsync(requestPath);
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<ApiUserDto>();
            return user;
        }
    }
}
