using Microsoft.WindowsAzure.Mobile.Service;

namespace FBClone.DataObjects
{
    public class Survey : EntityData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}