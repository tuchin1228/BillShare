namespace BillShare.DTO
{
    public class AddUserDTO
    {
        public Guid validateCode { get; set; }
        public string userId { get; set; }
        public int groupId { get; set; }
        public string password { get; set; }
    }
}
