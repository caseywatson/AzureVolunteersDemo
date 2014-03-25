using Microsoft.WindowsAzure.Storage.Table;

namespace StorageRole
{
    public class VolunteerTableEntity : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}