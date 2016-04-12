using Newtonsoft.Json;

namespace MailChimp.Resources {
    public class CampaignDefaults
    {
        [JsonProperty("from_name")]
        public string FromName { get; set; }

        [JsonProperty("from_email")]
        public string FromEmail { get; set; }
        
        public string Subject { get; set; }
        
        public string Language { get; set; }
    }
}