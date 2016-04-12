using System.Collections.Generic;
using Newtonsoft.Json;

namespace MailChimp.Resources
{
    public class ListMembers
    {
        public List<Member> Members { get; set; }

        [JsonProperty("list_id")]
        public string ListId { get; set; }

        [JsonProperty("total_items")]
        public int TotalItems { get; set; }
        
        public List<Link> Links { get; set; }
    }
}