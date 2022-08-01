using System.Text.Json.Serialization;

namespace BillShare.DTO
{
    public class AddGroupDTO
    {
        public string userId { get; set; }

        public string GroupName { get; set; }

        public string? GroupAnnouncement { get; set; }

        public IFormFile? formFile { get; set; }
        //public string? GroupBanner { get; set; }


    }
}
