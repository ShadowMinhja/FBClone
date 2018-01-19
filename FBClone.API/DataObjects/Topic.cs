using Microsoft.WindowsAzure.Mobile.Service;

namespace FBClone.DataObjects
{
    public class Topic : EntityData
    {
        public string TopicName { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}