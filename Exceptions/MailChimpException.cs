using System;

namespace MailChimp.Exceptions
{
    public class MailChimpException : Exception {

        public MailChimpProblem Problem { get; set; }

        public MailChimpException(string message)
            : base(message) {}

        public MailChimpException(string message, Exception innerException)
            : base(message, innerException) { }

        public MailChimpException(string message, MailChimpProblem problem)
            : base(message)
        {
            Problem = problem;
        }

        public MailChimpException(string message, Exception innerException, MailChimpProblem problem)
            : base(message, innerException)
        {
            Problem = problem;
        }

    }
}