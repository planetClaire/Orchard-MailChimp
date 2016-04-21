using System.Collections.Generic;

namespace MailChimp.Resources {
    public class Operation {
        public string Method { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> Params { get; set; }
        public string Body { get; set; }
        public string OperationId { get; set; }
    }
}