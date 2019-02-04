using Xamarin.Forms;
using ImpressoApp.UserControls.Walktrought;
using ImpressoApp.Enums;

namespace ImpressoApp.DataTemplateSelectors
{
    public class WalkthroughtCarouselViewTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BlockchainViewTemplate { get; set; }
        public DataTemplate BrandingYouViewTemplate { get; set; }
        public DataTemplate SmartProfileViewTemplate { get; set; }
        public DataTemplate QualityNetworkingViewTemplate { get; set; }
        public DataTemplate RecruitmentViewTemplate { get; set; }

        public WalkthroughtCarouselViewTemplateSelector()
        {
            BlockchainViewTemplate = new DataTemplate(typeof(BlockchainView));
            BrandingYouViewTemplate = new DataTemplate(typeof(BrandingView));
            SmartProfileViewTemplate = new DataTemplate(typeof(SmartProfileView));
            QualityNetworkingViewTemplate = new DataTemplate(typeof(QualityNetworkingView));
            RecruitmentViewTemplate = new DataTemplate(typeof(RecruitmentView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var viewType = (WalkthroughCarouselViewType)item;

            switch (viewType)
            {
                case WalkthroughCarouselViewType.Blockchain:
                    return BlockchainViewTemplate;
                case WalkthroughCarouselViewType.BrandingYou:
                    return BrandingYouViewTemplate;
                case WalkthroughCarouselViewType.SmartProfile:
                    return SmartProfileViewTemplate;
                case WalkthroughCarouselViewType.QualityNetworking:
                    return QualityNetworkingViewTemplate;
                case WalkthroughCarouselViewType.Recruitment:
                    return RecruitmentViewTemplate;
            }

            return BlockchainViewTemplate;
        }
    }
}
