using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MailChimp.Serialization {
    public class MailChimpSerializerSettings : JsonSerializerSettings {

        public MailChimpSerializerSettings() {
            NullValueHandling = NullValueHandling.Ignore;
            ContractResolver = new SnakeCaseContractResolver();
            Converters.Add(new StringEnumConverter { CamelCaseText = true });
        }
    }

}