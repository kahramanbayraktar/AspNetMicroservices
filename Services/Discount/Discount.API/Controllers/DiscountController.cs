using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository;
        }

        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [HttpGet("{productName}", Name = "GetDiscount")]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            return await _repository.GetDiscount(productName);
        }

        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            // TODO: return value check missing
            return CreatedAtRoute("GetDiscount", new Coupon { ProductName = coupon.ProductName }, coupon); // TODO: magic strings
        }

        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        [HttpPut]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscount(coupon));
        }

        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [HttpDelete("{productName}")]
        public async Task<ActionResult<bool>> DeleteDiscount(string productName)
        {
            return await _repository.DeleteDiscount(productName);
        }
    }
}
