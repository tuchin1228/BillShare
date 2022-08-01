using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillShare.Models
{
    public class Checkout
    {
        public int Id { get; set; }
        public string SendUserId { get; set; }
        public string ReceiveUserId { get; set; }
        public decimal amount { get; set; }
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public Group Group { get; set; }

        public int GroupId { get; set; }


    }
}
