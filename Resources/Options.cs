namespace MailChimp.Resources {
    public class Options : MailChimpResource
    {
        public int DefaultCountry { get; set; }
        public PhoneFormat PhoneFormat { get; set; }
        public string DateFormat { get; set; }
        public string[] Choices { get; set; }
        public int Size { get; set; }
    }

    public enum PhoneFormat
    {
        US,
        International
    }
}