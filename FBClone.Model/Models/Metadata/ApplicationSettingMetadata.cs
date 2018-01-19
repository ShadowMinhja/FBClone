using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FBClone.Model
{
    [MetadataType(typeof(ApplicationSettingMetadata))]
    public partial class ApplicationSetting
    {
        private sealed class ApplicationSettingMetadata
        {

        }

        public void UpdateFrom(ApplicationSetting applicationSetting)
        {
            this.StaffSetup = applicationSetting.StaffSetup;
            this.SurveySetup = applicationSetting.SurveySetup;
            this.PromotionSetup = applicationSetting.PromotionSetup;
            this.BrandingSetup = applicationSetting.BrandingSetup;
            this.SurveyIsLooping = applicationSetting.SurveyIsLooping;
            this.CreatedAt = applicationSetting.CreatedAt;
            this.CreatedBy = applicationSetting.CreatedBy;
            this.UpdatedAt = applicationSetting.UpdatedAt;
            this.UpdatedBy = applicationSetting.UpdatedBy;
            this.Version = applicationSetting.Version;
        }
    }
}