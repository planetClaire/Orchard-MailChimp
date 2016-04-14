using System.Collections.Generic;
using System.Text;

namespace MailChimp.Exceptions {
    public class MailChimpProblem
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
        public List<Error> Errors { get; set; }

        public override string ToString() {
            var result = new StringBuilder(string.Format("{0}, Status: {1}, Detail: {2} {3}", Title, Status, Detail, Type));
            if (!string.IsNullOrEmpty(Instance)) {
                result.AppendFormat(", Instance: {0}", Instance);
            }
            foreach (var error in Errors) {
                result.AppendFormat(" Field {0}: {1}. ", error.Field, error.Message);
            }
            return result.ToString();
        }
    }

    public class Error
    {
        public string Field { get; set; }
        public string Message { get; set; }
    }

}