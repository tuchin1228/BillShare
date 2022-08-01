using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillShare.Models
{
    public class Group_User
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        [JsonIgnore]
        public Group Group { get; set; }

        public string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }
        public bool admin { get; set; }
    }
}
