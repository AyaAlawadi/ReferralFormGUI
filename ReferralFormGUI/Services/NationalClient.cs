using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace ReferralFormGUI.Services
{
    public class NationalClient : INationalClient
    {
        private static readonly HttpClient _client = new HttpClient();
        private static string _baseUrl;
        private static string _clientId;
        private static string _secret;
        private static string _tenantId;
        private static string _resourceUrl;

        public NationalClient(IOptions<WebApiOptions> options)
        {
            _secret = options.Value.Secret;
            _tenantId = options.Value.TenantId;
            _clientId = options.Value.ClientId;
            _resourceUrl = options.Value.ResourceUrl;
            _baseUrl = "https://rbnationalcrmdev.azurewebsites.net";
        }

        public async Task<string> GetAccessToken()
        {
            var credentials = new ClientCredential(_clientId, _secret);

            var authContext = new AuthenticationContext("https://login.microsoftonline.com/" + _tenantId);

            var result = await authContext.AcquireTokenAsync(_resourceUrl, credentials);

            return result.AccessToken;



        }

        private async Task<TResp> SendRequest<TResp>(HttpRequestMessage req)
        {
            var token = await GetAccessToken();
            var authHeader = new AuthenticationHeaderValue("Bearer", token);

            _client.DefaultRequestHeaders.Authorization = authHeader;


            TResp response;

            //Send request
            using (var resp = await _client.SendAsync(req))
            {
                var respContent = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    if (resp.StatusCode.Equals(HttpStatusCode.NotFound))
                    {
                        return default;
                    }

                    var msg =
                        $"The CRM Rest endpoint returned a {resp.StatusCode} {resp.ReasonPhrase} status when attempting to {req.Method} {req.RequestUri}. Response contents were: {respContent}";

                    // TODO log
                    //throw new CRMRestApiException(msg);
                }
                try
                {
                    var res = JsonConvert.DeserializeObject(respContent);

                    response = JsonConvert.DeserializeObject<TResp>(respContent);

                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.Message);
                }
                return response;
            }
        }

        public async Task<TResp> Get<TReq, TResp>(NationalWebApiRequest<TReq> request)
        {
            try
            {
                using (var req = new HttpRequestMessage(HttpMethod.Get, _baseUrl + request.Endpoint)) // TODO only get neede attributes
                {
                    req.Headers.Add("Prefer", "odata.include-annotations=OData.Community.Display.V1.FormattedValue");
                    return await SendRequest<TResp>(req);
                }
            }
            catch (Exception m)
            {
            }
            throw new Exception($"Call to API failed after tries");
        }


    }
}

public class WebApiOptions
{
    public string ClientId { get; set; }
    public string Secret { get; set; }
    public string TenantId { get; set; }
    public string ResourceUrl { get; set; }
}
