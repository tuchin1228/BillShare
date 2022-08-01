using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillShare.Models
{
    public class ExpendDetail
    {
        public int Id { get; set; }

        public int type { get; set; }
        
        public decimal price { get; set; }

        public int ExpendId { get; set; }

        [JsonIgnore]
        public Expend Expend { get; set; }


        public string UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }


    }
}
