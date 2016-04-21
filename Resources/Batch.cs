using System.Collections.Generic;

namespace MailChimp.Resources
{
    public class Batch : MailChimpResource
    {
        public List<Operation> Operations { get; set; }

        public string Id { get; set; }
        public BatchStatus? Status { get; set; }
        public int? TotalOperations { get; set; }
        public int? FinishedOperations { get; set; }
        public int? ErroredOperations { get; set; }
        public string SubmittedAt { get; set; }
        public string CompletedAt { get; set; }
        public string ResponseBodyUrl { get; set; }
        public List<Link> Links { get; set; }
    }

    public enum BatchStatus {
        Pending,
        Started,
        Finished
    }
}