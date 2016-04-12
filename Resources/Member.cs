using System.Collections.Generic;
using Newtonsoft.Json;

namespace MailChimp.Resources
{
    public class Member
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("unique_email_id")]
        public string UniqueEmailId { get; set; }
        
        [JsonProperty("email_type")]
        public string EmailType { get; set; }
        
        public Status Status { get; set; }

        [JsonProperty("merge_fields")]
        public Dictionary<string, string> MergeFields { get; set; }
        
        public Stats Stats { get; set; }
        
        [JsonProperty("ip_signup")]
        public string IpSignup { get; set; }
        
        [JsonProperty("timestamp_signup")]
        public string TimestampSignup { get; set; }
        
        [JsonProperty("ip_opt")]
        public string IpOpt { get; set; }
        
        [JsonProperty("timestamp_opt")]
        public string TimestampOpt { get; set; }
        
        [JsonProperty("member_rating")]
        public int MemberRating { get; set; }
        
        [JsonProperty("last_changed")]
        public string LastChanged { get; set; }
        
        public string Language { get; set; }
        
        public bool Vip { get; set; }
        
        [JsonProperty("email_client")]
        public string EmailClient { get; set; }
        
        public Location location { get; set; }
        
        [JsonProperty("list_id")]
        public string ListId { get; set; }
        
        public List<Link> Links { get; set; }
    
    }

    public enum EmailType {
        Html,
        Text
    }

    public enum Status {
        Subscribed,
        Unsubscribed,
        Cleaned,
        Pending
    }
}