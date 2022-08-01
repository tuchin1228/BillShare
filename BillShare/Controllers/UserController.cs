using BillShare.Data;
using BillShare.DTO;
using BillShare.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BillShare.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
   
    public class UserController : Controller
    {

        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //取得某群組的所有成員
        [HttpGet("{GroupId}")]
        public IActionResult GetGroupUsers(int GroupId)
        {
            var users  = _context.Group_Users.Where(m => m.GroupId == GroupId)
                        .Include(m => m.User)
                        .Select(m=> new
                        {
                            m.UserId,
                            m.admin,
                            m.User.UserName
                        })
                        .ToList();
            return Ok(new
            {
                success = true,
                users = users
            });
        }

        //檢查是否為會員
        [HttpPost]
        public IActionResult CheckUser([FromBody]string userId)
        {
            var userCheck = _context.Users.FirstOrDefault(m => m.UserId == userId);

            if(userCheck == null)
            {
                return Ok(new
                {
                    success = false,
                    msg="無此會員"
                });
            }
            else
            {
                return Ok(new
                {
                    success = true,
                    msg = "已成為會員",
                    user = userCheck
                });
            }

        }

        //註冊會員
        [HttpPost]
        public IActionResult RegisterUser( RegisterUser userObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("參數錯誤");
            }
          
            if ( userObj.UserId == "" || userObj.Password == "" || userObj.UserName == "")
            {
                return BadRequest("參數錯誤");
            }
            var userCheck = _context.Users.FirstOrDefault(m => m.UserId == userObj.UserId);

            //var userCheck = _context.Users.FirstOrDefault(m => m.UserId == userObj.userId);

            if (userCheck != null)
            {
                return Ok(new
                {
                    success = false,
                    msg = "已成為會員"
                });
            }

            var data = new User
            {
                UserId = userObj.UserId,
                Password = userObj.Password,
                UserName = userObj.UserName
            };

            _context.Add(data);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = "註冊成功"
            });


        }

        //群組新增成員
        [HttpPost]
        public IActionResult AddGroupUsers(AddUserDTO userObj)
        {
            //if (!ModelState.IsValid )
            //{
            //    return BadRequest("參數錯誤");
            //}    
            //    return Ok(userObj);
            

            var groupCheck = _context.Groups.FirstOrDefault(m => m.GroupId == userObj.groupId && m.ValidateCode == userObj.validateCode);

            if (groupCheck == null)
            {
                return BadRequest("群組驗證錯誤");
            }

            var userCheck = _context.Users.FirstOrDefault(m => m.Password == userObj.password && m.UserId == userObj.userId);
            
            if (userCheck == null)
            {
                return BadRequest("帳號密碼錯誤");
            }

            var exitCheck = _context.Group_Users.FirstOrDefault(m => m.UserId == userObj.userId);

            if (exitCheck != null)
            {
                return Ok(new
                {
                    success = false,
                    msg = "會員已在群組"
                });
            }

            var data = new Group_User
            {
                GroupId = userObj.groupId,
                UserId = userObj.userId,
                admin = false
            };
            _context.Group_Users.Add(data);
            _context.SaveChanges();


            return Ok(new
            {
                success = true
            });
        }

    }

}
