using BillShare.Data;
using BillShare.DTO;
using BillShare.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillShare.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        private readonly IWebHostEnvironment _env;

        private readonly DataContext _context;

        public CheckoutController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


       [HttpPost]
        public IActionResult Checkout(CheckoutDTO checkoutObj)
        {
            var checkout = new Checkout
            {
                SendUserId = checkoutObj.SendUserId,
                ReceiveUserId = checkoutObj.ReceiveUserId,
                amount = checkoutObj.amount,
                GroupId = checkoutObj.GroupId,
                //CreatedDate = checkoutObj.CreatedDate
                CreatedDate = DateTime.Now
            };

            _context.Checkouts.Add(checkout);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = "結帳新增成功"
            });
        }

        [HttpGet("{GroupId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetCheckoutReceive(int GroupId)
        {

            var receiveData = _context.Checkouts.Where(m => m.GroupId == GroupId).ToList();

            return Ok(new
            {
                success = true,
                receiveData = receiveData
            });
        }



        [HttpPost("{CheckoutId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteCheckout(int CheckoutId)
        {
            var CheckoutData = _context.Checkouts.FirstOrDefault(m => m.Id == CheckoutId);
            if(CheckoutData == null)
            {
                return Ok(new
                {
                    success = false,
                    msg = "無此結帳"
                });
            }

            _context.Remove(CheckoutData);
            _context.SaveChanges();

            return Ok(new
            {
                success = true,
                msg = "結帳刪除成功"
            });
        }

    }
}
