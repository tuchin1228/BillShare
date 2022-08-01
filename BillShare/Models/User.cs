using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillShare.Models
{
    public class User
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        //public string LineId { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<Group_User>? Group_User { get; set; }

        [JsonIgnore]
        public ICollection<ExpendDetail>? ExpendDetail { get; set; }

    }
}
