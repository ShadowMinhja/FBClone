using Microsoft.WindowsAzure.Mobile.Service;

namespace FBClone.DataObjects
{
    public class ApplicationSetting : EntityData
    {
        public bool StaffSetup { get; set; }
        public bool SurveySetup { get; set; }
        public bool PromotionSetup { get; set; }
        public bool BrandingSetup { get; set; }
        public bool SurveyIsLooping { get; set; }
        public string BrandingLogoURL { get; set; }
        public string PromoLogoURL { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}