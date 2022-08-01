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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpendController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly DataContext _context;

        public ExpendController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        //某群組中的所有花費
        [HttpGet("{GroupId}")]
        public IActionResult Expends(int GroupId)
        {
            if (GroupId == null)
            {
                return BadRequest("參數錯誤");
            }

            var expends = _context.Expends.Where(m => m.GroupId == GroupId)
                .ToList();
            //var expends = _context.Expends.Where(m => m.GroupId == GroupId)
            //    .Include(m => m.ExpendDetail)
            //      .Select(m => new
            //      {
            //          m.ExpendId,
            //          m.ItemName,
            //          m.TotalAmount,
            //          m.remark,
            //          m.CreatedDate,
            //          m.GroupId,
            //          m.ExpendDetail
            //      }).ToList();

            return Ok(new
            {
                success = true,
                expends = expends
            });
        }

        //群組中某項花費包含誰付誰分
        [HttpGet("{GroupId}/{ExpendId}")]
        public IActionResult ExpendDetails(int GroupId, int ExpendId)
        {
            if (GroupId == null)
            {
                return BadRequest("參數錯誤");
            }

            var expend = _context.Expends.FirstOrDefault(m => m.GroupId == GroupId && m.ExpendId == ExpendId);
            var expendDetail = _context.ExpendDetails
                                .Include(m => m.User)
                                .Where(m => m.ExpendId == ExpendId)
                                .Select(m => new
                                {
                                    m.Id,
                                    m.type,
                                    m.price,
                                    m.User.UserName
                                }).ToList();
            return Ok(new
            {
                success = true,
                expend = expend,
                expendDetail = expendDetail
            });
        }


        //群組中所有花費包含誰付誰分
        [HttpGet("{GroupId}")]
        public IActionResult AllExpendDetails(int GroupId)
        {
            if (GroupId == null)
            {
                return BadRequest("參數錯誤");
            }

            var expends = _context.Expends.Where(m => m.GroupId == GroupId).ToList();
            var expendDetail = _context.ExpendDetails
                                .Include(m => m.User)
                                .Include(m => m.Expend)
                                .Where(m => m.Expend.GroupId == GroupId)
                                .Select(m => new
                                {
                                    m.Id,
                                    m.type,
                                    m.price,
                                    m.User.UserId,
                                    m.User.UserName,
                                    m.ExpendId
                                }).ToList();
            return Ok(new
            {
                success = true,
                expends = expends,
                expendDetail = expendDetail
            });
        }


        //會員新增花費分帳
        [HttpPost]
        public IActionResult Expend(ExpendDTO expendObj)
        {
            //return Ok(expendObj);
            var twtzinfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
            DateTime createDate = TimeZoneInfo.ConvertTimeFromUtc(expendObj.CreatedDate, twtzinfo);
            var expend = new Expend
            {
                ItemName = expendObj.ItemName,
                TotalAmount = expendObj.TotalAmount,
                remark = expendObj.remark,
                GroupId = expendObj.GroupId,
                CreatedDate = createDate
            };

            _context.Expends.Add(expend);
            _context.SaveChanges();

            foreach (var expendDetail in expendObj.ExpendDetail)
            {
                var detail = new ExpendDetail
                {
                    type = expendDetail.type,
                    price = expendDetail.price,
                    ExpendId = expend.ExpendId,
                    UserId = expendDetail.UserId
                };
                _context.ExpendDetails.Add(detail);
            }
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = "花費新增成功"
            });
        }


        //刪除花費
        [HttpPost("{ExpendId}")]
        public IActionResult DeleteExpend(int ExpendId)
        {
            var checkExpend = _context.Expends.FirstOrDefault(m => m.ExpendId == ExpendId);
            if (checkExpend == null)
            {
                return Ok(new
                {
                    success = false,
                    msg = "花費參數錯誤"
                });
            }

            _context.Expends.Remove(checkExpend);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = "花費刪除成功"
            });
        }




    }
}
