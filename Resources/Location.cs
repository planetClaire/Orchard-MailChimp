namespace MailChimp.Resources {
    public class Location : MailChimpResource
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int Gmtoff { get; set; }
        public int Dstoff { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
    }
}