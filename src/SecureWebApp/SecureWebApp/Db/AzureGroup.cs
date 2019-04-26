using System.ComponentModel.DataAnnotations.Schema;

namespace SecureWebApp.Db
{
    [Table("AzureGroups")]
    public class AzureGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string MeetupUrl { get; set; } 
    }
}
