namespace MailChimp.Resources {
    public class Link {
        public string Rel { get; set; }
        public string Href { get; set; }
        public Method Method { get; set; }
        public string TargetSchema { get; set; }
        public string Schema { get; set; }
    }

    public enum Method
    {
        GET,
        POST,
        PATCH,
        PUT,
        DELETE
    }

}