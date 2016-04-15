using System.Collections.Generic;

namespace MailChimp.Resources
{
    public class BatchCollection {
        public List<Batch> Batches { get; set; }
        public int TotalItems { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Batch
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

    public class Operation {
        public string Method { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Params { get; set; }
        public string Body { get; set; }
        public string OperationId { get; set; }
    }
}