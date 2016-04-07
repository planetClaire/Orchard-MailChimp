using System;
using System.Security.Cryptography;
using System.Text;
using MailChimp.Models;
using MailChimp.Resources;
using Orchard;
using Orchard.ContentManagement;
using RestSharp;
using RestSharp.Authenticators;

namespace MailChimp.Services
{
    public class MailChimpService : IMailChimpService
    {
        private readonly string _apiKey;
        private readonly string _dataCenter;

        private const string BaseUrl = "https://{0}.api.mailchimp.com/3.0";

        public MailChimpService(IWorkContextAccessor workContext) {
            _apiKey = workContext.GetContext().CurrentSite.As<MailChimpSettingsPart>().MailChimpApiKey;
            _dataCenter = _apiKey.Substring(_apiKey.IndexOf('-') + 1);
        }

        public Subscriber GetSubscriber(string listId, string emailAddress)
        {
            var request = new RestRequest
            {
                Resource = "lists/{list_id}/members/{subscriber_hash}",
                RootElement = "Subscriber"
            };

            request.AddParameter("list_id", listId, ParameterType.UrlSegment);
            request.AddParameter("subscriber_hash", CreateMD5(emailAddress), ParameterType.UrlSegment);

            return Execute<Subscriber>(request);
        }

        private T Execute<T>(IRestRequest request) where T : new()
        {
            var client = new RestClient
            {
                BaseUrl = new Uri(string.Format(BaseUrl, _dataCenter)),
                Authenticator = new HttpBasicAuthenticator("user", _apiKey)
            };
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var mailchimpException = new ApplicationException(message, response.ErrorException);
                throw mailchimpException;
            }
            return response.Data;
        }

        private static string CreateMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input.ToLowerInvariant());
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var t in hashBytes) {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        
        //public void GetListSubscribers(string idList) {
        //    var apiKey = _workContext.GetContext().CurrentSite.As<MailChimpSettingsPart>().MailChimpApiKey;
        //    var client = GetRestClient(apiKey);
        //    var request = new RestRequest(string.Format("lists/{0}/members", idList));
        //    var response = client.Execute(request);
        //    var content = response.Content;
        //}

    }
}