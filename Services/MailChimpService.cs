using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailChimp.Exceptions;
using MailChimp.Models;
using MailChimp.Resources;
using MailChimp.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Orchard;
using Orchard.Caching;
using Orchard.ContentManagement;

namespace MailChimp.Services
{
    public class MailChimpService : IMailChimpService
    {
        private const string BaseUrl = "https://{0}.api.mailchimp.com";
        private const string ApiVersion = "3.0";
        private const string MembersListSignal = "MailChimpMembersList";

        private readonly string _apiKey;
        private readonly string _dataCenter;
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private HttpClient _mailChimpHttpClient;

        public MailChimpService(IWorkContextAccessor workContext, ICacheManager cacheManager, ISignals signals)
        {
            _cacheManager = cacheManager;
            _signals = signals;
            _apiKey = workContext.GetContext().CurrentSite.As<MailChimpSettingsPart>().MailChimpApiKey;
            _dataCenter = _apiKey.Substring(_apiKey.IndexOf('-') + 1);
        }

        private static JsonSerializerSettings JsonSerializerSettings
        {
            get
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new SnakeCaseContractResolver()
                };
                serializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return serializerSettings;
            }
        }

        private HttpClient MailChimpHttpClient
        {
            get {
                return _mailChimpHttpClient ?? CreateHttpClient();
            }
        }

        public async Task<Member> GetMember(string listId, string emailAddress)
        {
            var endpoint = string.Format("{0}/lists/{1}/members/{2}", ApiVersion, listId, CreateMD5(emailAddress));
            var failureMessage = string.Format("Failed to get member {0} from list {1}", emailAddress, listId);
            return await GetAsync<Member>(endpoint, failureMessage);
        }

        public async Task<List> GetList(string listId, string[] fields = null) {
            var endpoint = string.Format("{0}/lists/{1}{2}", ApiVersion, listId, FieldsString(fields));
            var failureMessage = string.Format("Failed to get list {0}", listId);
            return await GetAsync<List>(endpoint, failureMessage);
        }

        public async Task<ListMembers> GetMembersInfo(string idList, string[] fields = null) {
            var endpoint = string.Format("{0}/lists/{1}/members{2}", ApiVersion, idList, FieldsString(fields));
            var failureMessage = string.Format("Failed to get members info from list {0}", idList);
            return await GetAsync<ListMembers>(endpoint, failureMessage);
        }

        public async Task<ListMembers> GetAllMembers(string listId)
        {
            return  await _cacheManager.Get(string.Format("{0}{1}", MembersListSignal, listId), async ctx => {
                ctx.Monitor(_signals.When(string.Format("{0}{1}Changed", MembersListSignal, listId)));
            
                var membersInfo = await GetMembersInfo(listId, new[] {"total_items"});
                var endpoint = string.Format("{0}/lists/{1}/members?count={2}", ApiVersion, listId, membersInfo.TotalItems);
                var failureMessage = string.Format("Failed to get members from list {0}", listId);
                return await GetAsync<ListMembers>(endpoint, failureMessage);
            
            });
        }

        public async Task<Member> AddMember(Member member) {
            var endpoint = string.Format("{0}/lists/{1}/members", ApiVersion, member.ListId);
            var failureMessage = string.Format("Failed to add member {0} to list {1}", member.EmailAddress, member.ListId);
            return await PostAsync(endpoint, failureMessage, member, new List<string> {string.Format("{0}{1}Changed", MembersListSignal, member.ListId)});
        }

        public async Task<Member> AddOrUpdateMember(Member member) {
            var endpoint = string.Format("{0}/lists/{1}/members/{2}", ApiVersion, member.ListId, CreateMD5(member.EmailAddress));
            var failureMessage = string.Format("Failed to add or update member {0} on list {1}", member.EmailAddress, member.ListId);
            return await PutAsync(endpoint, failureMessage, member, new List<string> { string.Format("{0}{1}Changed", MembersListSignal, member.ListId) });
        }

        public async Task<bool> DeleteMember(string idList, string emailAddress) {
            var endpoint = string.Format("{0}/lists/{1}/members/{2}", ApiVersion, idList, CreateMD5(emailAddress));
            var response = await MailChimpHttpClient.DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode) {
                _signals.Trigger(string.Format("{0}{1}Changed", MembersListSignal, idList));
                return true;
            }
            throw new MailChimpException(string.Format("Failed to delete member {0} from list {1}", emailAddress, idList));
        }

        public async Task<Batch> CreateBatch(List<Member> membersToPut) {
            var operations = ConstructOperations(membersToPut);
            var batch = new Batch {Operations = operations};
            return await PostAsync(string.Format("{0}/batches", ApiVersion), "Failed to create batch", batch);
        }

        public async Task<Batch> CreateBatch(List<Member> listMembersToPut, List<Member> listMembersToDelete) {
            var operations = ConstructOperations(listMembersToPut);
            foreach (var member in listMembersToDelete) {
                operations.Add(new Operation {
                    Method = Method.DELETE.ToString(),
                    OperationId = member.EmailAddress,
                    Path = string.Format("/lists/{0}/members/{1}", member.ListId, CreateMD5(member.EmailAddress))
                });
            }
            var batch = new Batch { Operations = operations };
            return await PostAsync(string.Format("{0}/batches", ApiVersion), "Failed to create batch", batch);
        }

        public async Task<BatchCollection> GetBatches() {
            return await GetAsync<BatchCollection>(string.Format("{0}/batches", ApiVersion), "Failed to get batches");
        }

        public async Task<BatchCollection> GetAllBatches() {
            var batches = await GetBatches();
            var totalCount = batches.TotalItems;
            return await GetAsync<BatchCollection>(string.Format("{0}/batches?count={1}", ApiVersion, totalCount), "Failed to get batches");
        }

        public void RefreshCache(string idList) {
            _signals.Trigger(string.Format("{0}{1}Changed", MembersListSignal, idList));
        }

        private static List<Operation> ConstructOperations(List<Member> membersToPut)
        {
            var operations = membersToPut.Select(member => new Operation
            {
                OperationId = member.EmailAddress,
                Method = Method.PUT.ToString(),
                Path = string.Format("/lists/{0}/members/{1}",
                    member.ListId, CreateMD5(member.EmailAddress)),
                Body = JsonConvert.SerializeObject(member, JsonSerializerSettings)
            }).ToList();
            return operations;
        }

        private static string FieldsString(string[] fields)
        {
            var result = "";
            if (fields != null && fields.Any())
            {
                result += "?fields=" + string.Join(",", fields);
            }
            return result;
        }

        private static async Task<T> ProcessResponse<T>(string failureMessage, HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), JsonSerializerSettings);
            }
            if (response.Content != null)
            {
                var problem = JsonConvert.DeserializeObject<MailChimpProblem>(await response.Content.ReadAsStringAsync(), JsonSerializerSettings);
                throw new MailChimpException(string.Format("{0}: {1}", failureMessage, problem));
            }
            throw new MailChimpException(failureMessage);
        }

        private static string CreateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input.ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes)
                {
                    sb.Append(t.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private HttpClient CreateHttpClient()
        {
            _mailChimpHttpClient = new HttpClient
            {
                BaseAddress = new Uri(string.Format(BaseUrl, _dataCenter))
            };
            _mailChimpHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("apikey:{0}", _apiKey))));
            return _mailChimpHttpClient;
        }

        private async Task<T> GetAsync<T>(string endpoint, string failureMessage)
        {
            var response = await MailChimpHttpClient.GetAsync(endpoint);
            return await ProcessResponse<T>(failureMessage, response);
        }

        private async Task<T> PostAsync<T>(string endpoint, string failureMessage, T content, List<string> triggerSignals = null) {
            var response = await MailChimpHttpClient.PostAsync(endpoint, new StringContent(JsonConvert.SerializeObject(content, JsonSerializerSettings)));
            if (triggerSignals != null && triggerSignals.Any()) {
                foreach (var signal in triggerSignals) {
                    _signals.Trigger(signal);
                }
            }
            return await ProcessResponse<T>(failureMessage, response);
        }

        private async Task<T> PutAsync<T>(string endpoint, string failureMessage, T content, List<string> triggerSignals = null)
        {
            var response = await MailChimpHttpClient.PutAsync(endpoint, new StringContent(JsonConvert.SerializeObject(content, JsonSerializerSettings)));
            if (triggerSignals != null && triggerSignals.Any())
            {
                foreach (var signal in triggerSignals)
                {
                    _signals.Trigger(signal);
                }
            }
            return await ProcessResponse<T>(failureMessage, response);
        }

    }
}
