namespace MailChimp.Resources {
    public class Location : MailChimpResource
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Gmtoff { get; set; }
        public float Dstoff { get; set; }
        public string CountryCode { get; set; }
        public string Timezone { get; set; }
    }
}