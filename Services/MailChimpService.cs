using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailChimp.Exceptions;
using MailChimp.Models;
using MailChimp.Resources;
using Newtonsoft.Json;
using Orchard;
using Orchard.ContentManagement;

namespace MailChimp.Services
{
    public class MailChimpService : IMailChimpService
    {
        private readonly string _apiKey;
        private readonly string _dataCenter;

        private const string BaseUrl = "https://{0}.api.mailchimp.com";
        private const string ApiVersion = "3.0";

        private HttpClient MailChimpHttpClient
        {
            get
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(string.Format(BaseUrl, _dataCenter))
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("apikey:{0}", _apiKey))));
                return client;
            }
        }

        public MailChimpService(IWorkContextAccessor workContext) {
            _apiKey = workContext.GetContext().CurrentSite.As<MailChimpSettingsPart>().MailChimpApiKey;
            _dataCenter = _apiKey.Substring(_apiKey.IndexOf('-') + 1);
        }

        public async Task<Subscriber> GetSubscriber(string listId, string emailAddress)
        {
            var endpoint = string.Format("{0}/lists/{1}/members/{2}", ApiVersion, listId, CreateMD5(emailAddress));

            using (MailChimpHttpClient)
            {
                var response = await MailChimpHttpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode) {
                    return await response.Content.ReadAsAsync<Subscriber>();
                }
                var message = string.Format("Failed to get member {0} from list {1}", emailAddress, listId);
                if (response.Content != null) {
                    var problem = JsonConvert.DeserializeObject<MailChimpProblem>(await response.Content.ReadAsStringAsync());
                    throw new MailChimpException(message, problem);
                }
                throw new MailChimpException(message);
            }
        }

        public Subscriber AddOrUpdateSubscriber(Subscriber subscriber) {
            throw new NotImplementedException();
        }

        private static string CreateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input.ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes) {
                    sb.Append(t.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        
    }
}
