using MailChimp.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Localization;

namespace MailChimp.Handlers
{
    public class MailChimpSettingsPartHandler : ContentHandler {

        public MailChimpSettingsPartHandler()
        {
            T = NullLocalizer.Instance;
            Filters.Add(new ActivatingFilter<MailChimpSettingsPart>("Site"));
            Filters.Add(new TemplateFilterForPart<MailChimpSettingsPart>("MailChimpSettings", "Parts/MailChimpSettings", "MailChimp"));
        }

        public Localizer T { get; set; }

        protected override void GetItemMetadata(GetContentItemMetadataContext context) {
            if (context.ContentItem.ContentType != "Site")
                return;
            base.GetItemMetadata(context);
            context.Metadata.EditorGroupInfo.Add(new GroupInfo(T("MailChimp")));
        }
    }
}