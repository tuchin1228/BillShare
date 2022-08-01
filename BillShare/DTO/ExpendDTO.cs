using BillShare.Models;
using System.ComponentModel.DataAnnotations;

namespace BillShare.DTO
{
    public class ExpendDTO
    {
        public string ItemName { get; set; }
        public decimal TotalAmount { get; set; }
        public string? remark { get; set; }
        public int GroupId { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<ExpendDetailDTO>? ExpendDetail { get; set; }

    }
}
