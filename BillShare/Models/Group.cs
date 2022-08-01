using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BillShare.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }
        public Guid ValidateCode { get; set; }

        public string? GroupAnnouncement { get; set; }
        public string? GroupBanner { get; set; }

        [JsonIgnore]
        public ICollection<Group_User>? Group_User { get; set; }

        [JsonIgnore]
        public ICollection<Expend>? Expend { get; set; }

        [JsonIgnore]
        public ICollection<Checkout>? Checkout { get; set; }
    }
}
