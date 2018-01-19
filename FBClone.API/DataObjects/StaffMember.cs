using Microsoft.WindowsAzure.Mobile.Service;

namespace FBClone.DataObjects
{
    public class StaffMember : EntityData
    {
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Passcode { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}