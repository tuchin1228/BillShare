namespace BillShare.DTO
{
    public class CheckoutDTO
    {
        public string SendUserId { get; set; }
        public string ReceiveUserId { get; set; }
        public decimal amount { get; set; }
        //public DateTime CreatedDate { get; set; }
        public int GroupId { get; set; }

    }
}
