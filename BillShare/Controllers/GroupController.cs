using BillShare.Data;
using BillShare.DTO;
using BillShare.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace BillShare.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;

        public GroupController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //取得某會員所有群組
        [HttpGet("{UserId}")]
        public IActionResult Group(string UserId)
        {
            var JwtUserId = User.FindFirst("UserId").Value;
            if(JwtUserId != UserId)
            {
                return BadRequest("使用者驗證錯誤");
            }
            var groups = _context.Group_Users
                .Include(m => m.Group)
                .Where(m => m.UserId == UserId)
                .Select(m => new
                {
                    m.Id,
                    m.GroupId,
                    m.UserId,
                    m.Group.GroupName,
                    m.Group.GroupAnnouncement,
                    m.Group.GroupBanner,
                })
                .ToList();
            return Ok(new
            {
                success = true,
                groups = groups
            });
        }

        //取得群組資訊
        [HttpGet("{GroupId}")]
        public IActionResult GetGroup(int GroupId)
        {
            var group = _context.Groups.FirstOrDefault(m => m.GroupId == GroupId);
            return Ok(new
            {
                success = true,
                group = group
            });
        }

        //會員新增群組
        [HttpPost]
        public async Task<IActionResult> Group([FromForm]AddGroupDTO groupObj)
        {
         
            //return Ok(groupObj.formFile);
            if (groupObj.userId == null || groupObj.GroupName == null)
            {
                return BadRequest("參數錯誤");
            }
            var JwtUserId = User.FindFirst("UserId").Value;
            if (JwtUserId != groupObj.userId)
            {
                return BadRequest("使用者驗證錯誤");
            }

            var user = _context.Users.FirstOrDefault(m => m.UserId == groupObj.userId);
            if (user != null)
            {

                string rootPath = _env.ContentRootPath + @"\wwwroot\groupImages\";

                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                string fileName = "";
                string fullPath = "";

                if (groupObj.formFile != null && groupObj.formFile.Length > 0)
                {

                    fileName = $"{ RandomString(10)}.jpg";
                    fullPath = $"{rootPath + fileName}";
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await groupObj.formFile.CopyToAsync(stream);
                    }

                }

                var GroupAnnouncement = groupObj.GroupAnnouncement != null ? groupObj.GroupAnnouncement : null;

                var group = new Group { GroupName = groupObj.GroupName, ValidateCode = Guid.NewGuid(), GroupBanner = fileName , GroupAnnouncement = GroupAnnouncement };
                _context.Groups.Add(group);
                _context.SaveChanges();

                var group_user = new Group_User { UserId = groupObj.userId, GroupId = group.GroupId, admin = true };
                _context.Group_Users.Add(group_user);
                _context.SaveChanges();


                return Ok(new
                {
                    success = true,
                    msg = "群組新增成功"
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    msg = "會員資訊錯誤"
                });
            }

        }

        //會員編輯群組
        [HttpPost("{GroupId}")]
        public async Task<IActionResult> EditGroup([FromForm] AddGroupDTO groupObj,int GroupId)
        {

            //return Ok(groupObj.formFile);
            if (groupObj.userId == null || groupObj.GroupName == null)
            {
                return BadRequest("參數錯誤");
            }
            var JwtUserId = User.FindFirst("UserId").Value;
            if (JwtUserId != groupObj.userId)
            {
                return BadRequest("使用者驗證錯誤");
            }

            var user = _context.Users.FirstOrDefault(m => m.UserId == groupObj.userId);
            if (user != null)
            {

                var group = _context.Groups.FirstOrDefault(m => m.GroupId == GroupId);

                string rootPath = _env.ContentRootPath + @"\wwwroot\groupImages\";

                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                string fileName = "";
                string fullPath = "";

                if (groupObj.formFile != null && groupObj.formFile.Length > 0)
                {
                    FileInfo file = new FileInfo($"{rootPath + group.GroupBanner}");
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    fileName = $"{RandomString(10)}.jpg";
                    fullPath = $"{rootPath + fileName}";
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await groupObj.formFile.CopyToAsync(stream);
                    }


                }

                group.GroupName = groupObj.GroupName;
                group.GroupAnnouncement = groupObj.GroupAnnouncement != null ? groupObj.GroupAnnouncement : null;
                group.GroupBanner = groupObj.formFile != null && groupObj.formFile.Length > 0 ? fileName : group.GroupBanner;
                _context.SaveChanges();

                //var GroupAnnouncement = groupObj.GroupAnnouncement != null ? groupObj.GroupAnnouncement : null;

                //var group = new Group { GroupName = groupObj.GroupName, ValidateCode = Guid.NewGuid(), GroupBanner = fileName, GroupAnnouncement = GroupAnnouncement };
                //_context.Groups.Add(group);
                //_context.SaveChanges();

                //var group_user = new Group_User { UserId = groupObj.userId, GroupId = group.GroupId, admin = true };
                //_context.Group_Users.Add(group_user);
                //_context.SaveChanges();


                return Ok(new
                {
                    success = true,
                    msg = "群組編輯成功"
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    msg = "會員資訊錯誤"
                });
            }

        }

        //群組移除會員
        [HttpPost]
        public IActionResult RemoveGroupUser(RemoveGroupUserDTO userObj)
        {

            var JwtUserId = User.FindFirst("UserId").Value;
            var checkAdmin = _context.Group_Users.FirstOrDefault(m => m.GroupId == userObj.groupId && m.UserId == JwtUserId && m.admin == true);
            if(checkAdmin == null)
            {
                return Ok(new
                {
                    success = false,
                    msg = "身分驗證錯誤"
                });
            }

            var removeObj =  _context.Group_Users.FirstOrDefault(m => m.GroupId == userObj.groupId && m.UserId == userObj.userId);
            if(removeObj == null)
            {
                return Ok(new
                {
                    success = false,
                    msg = "無此成員"
                });
            }

            _context.Group_Users.Remove(removeObj);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = "會員移除成功"
            });
        }

        private string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

    }
}
