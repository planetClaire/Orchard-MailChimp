using System.Collections.Generic;

namespace MailChimp.Resources {
    public class BatchCollection : MailChimpResource
    {
        public List<Batch> Batches { get; set; }
        public int TotalItems { get; set; }
        public List<Link> Links { get; set; }
    }
}