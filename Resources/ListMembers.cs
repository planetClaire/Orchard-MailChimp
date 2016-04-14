using System.Collections.Generic;

namespace MailChimp.Resources
{
    public class ListMembers
    {
        public List<Member> Members { get; set; }
        public string ListId { get; set; }
        public int TotalItems { get; set; }
        public List<Link> Links { get; set; }
    }
}