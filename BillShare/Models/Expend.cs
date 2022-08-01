using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BillShare.Models
{
    public class Expend
    {
        public int ExpendId { get; set; }
        public string ItemName { get; set; }
        public decimal TotalAmount { get; set; }
        public string remark { get; set; }
        public DateTime CreatedDate { get; set; }

        public int GroupId { get; set; }

        [JsonIgnore]
        public Group Group { get; set; }

        [JsonIgnore]
        public ICollection<ExpendDetail>? ExpendDetail { get; set; }
    }
}
