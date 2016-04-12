using System.Collections.Generic;

namespace MailChimp.Resources {

    public class MergeField {
        public int MergeId { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool Public { get; set; }
        public int DisplayOrder { get; set; }
        public Options Options { get; set; }
        public string ListId { get; set; }
        public int TotalItems { get; set; }
        public List<Link> Links { get; set; }
    }

    public class Options {
        public int DefaultCountry { get; set; }
        public PhoneFormat PhoneFormat { get; set; }
        public string DateFormat { get; set; }
        public string[] Choices { get; set; }
        public int Size { get; set; }
    }

    public enum PhoneFormat {
        US,
        International
    }
}