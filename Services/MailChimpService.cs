using System;
using System.Linq;
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
using Orchard.Caching;
using Orchard.ContentManagement;

namespace MailChimp.Services
{
    public class MailChimpService : IMailChimpService
    {
        private readonly string _apiKey;
        private readonly string _dataCenter;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;

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

        public MailChimpService(IWorkContextAccessor workContext, ICacheManager cacheManager, ISignals signals) {
            _cacheManager = cacheManager;
            _signals = signals;
            _apiKey = workContext.GetContext().CurrentSite.As<MailChimpSettingsPart>().MailChimpApiKey;
            _dataCenter = _apiKey.Substring(_apiKey.IndexOf('-') + 1);
        }

        public async Task<Member> GetMember(string listId, string emailAddress)
        {
            var endpoint = string.Format("{0}/lists/{1}/members/{2}", ApiVersion, listId, CreateMD5(emailAddress));
            var failureMessage = string.Format("Failed to get member {0} from list {1}", emailAddress, listId);
            return await GetAsync<Member>(endpoint, failureMessage);
        }

        public async Task<List> GetList(string listId, string[] fields = null) {
            var endpoint = string.Format("{0}/lists/{1}", ApiVersion, listId);
            if (fields != null && fields.Any()) {
                endpoint += "?fields=" + string.Join(",", fields);
            }
            var failureMessage = string.Format("Failed to get list {0}", listId);
            return await GetAsync<List>(endpoint, failureMessage);
        }

        public async Task<ListMembers> GetMembers(string listId)
        {
            return  await _cacheManager.Get(string.Format("MailChimpMembersList{0}", listId), async ctx => {
                ctx.Monitor(_signals.When(string.Format("MailChimpMembersList{0}Changed", listId)));
            
                var list = await GetList(listId, new []{"stats"});
                var memberCount = list.Stats.MemberCount;
                var endpoint = string.Format("{0}/lists/{1}/members?count={2}", ApiVersion, listId, memberCount);
                var failureMessage = string.Format("Failed to get members from list {0}", listId);
                return await GetAsync<ListMembers>(endpoint, failureMessage);
            
            });
        }

        public async Task<Member> AddMember(Member member) {
            var endpoint = string.Format("{0}/lists/{1}/members", ApiVersion, member.ListId);
            var failureMessage = string.Format("Failed to add member {0} to list {1}", member.EmailAddress, member.ListId);
            return await PostAsync(endpoint, failureMessage, member);
        }

        public void RefreshCache(string idList)
        {
            _signals.Trigger(string.Format("MailChimpMembersList{0}Changed", idList));
        }

        private async Task<T> GetAsync<T>(string endpoint, string message)
        {
            using (MailChimpHttpClient) {
                var response = await MailChimpHttpClient.GetAsync(endpoint);
                return await ProcessResponse<T>(message, response);
            }
        }

        private async Task<T> PostAsync<T>(string endpoint, string failureMessage, T content) {
            using (MailChimpHttpClient) {
                var response = await MailChimpHttpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(content)));
                return await ProcessResponse<T>(failureMessage, response);
            }
        }

        private static async Task<T> ProcessResponse<T>(string message, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }
            if (response.Content != null)
            {
                var problem = JsonConvert.DeserializeObject<MailChimpProblem>(await response.Content.ReadAsStringAsync());
                throw new MailChimpException(string.Format("{0}: {1}", message, problem));
            }
            throw new MailChimpException(message);
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
