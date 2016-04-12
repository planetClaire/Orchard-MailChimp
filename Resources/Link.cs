namespace MailChimp.Resources {
    public class Link {
        
        public string Rel { get; set; }
        
        public string Href { get; set; }
        
        public Enums.Method Method { get; set; }
        
        public string TargetSchema { get; set; }
        
        public string Schema { get; set; }
    }
}