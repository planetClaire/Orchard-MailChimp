﻿namespace MailChimp.Exceptions {
    public class MailChimpProblem
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }
    }
}