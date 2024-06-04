

namespace B2CBackend
{
    public class HttpRequestReader
    {
        public InvocationContext GetInvocationContext(HttpRequest request)
        {
            var token = GetHeaderValue(request, "Authorization");
            var userId = GetHeaderValue(request, "Xmh-Userid");
            var apiVersion = GetHeaderValue(request, "Xmh-Apiversion");
            var hostname = GetHeaderValue(request, "Xmh-Hostname");
            var managementApiUrl = GetHeaderValue(request, "Xmh-Managementapiurl");
            var origin = GetHeaderValue(request, "Xmh-Origin");

            return new InvocationContext(token, userId, apiVersion, hostname, managementApiUrl, origin);
        }


        private string? GetHeaderValue(HttpRequest request, string headerName)
        {
            if (request.Headers.TryGetValue(headerName, out var headerValues))
            {
                return headerValues.FirstOrDefault();
            }
            return null;
        }
    }

    public record InvocationContext(
        string? Token,
        string? UserId, 
        string? ApiVersion, 
        string? Hostname, 
        string? ManagementApiUrl, 
        string? Origin);
}
