using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountService;

        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountService)
        {
            _basketRepository = basketRepository;
            _discountService = discountService;
        }

        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [HttpGet("{userName}", Name = "GetBasket")] // This use make the method work in this form: /basket/someusername
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Call Discount.Grpc
            foreach (var item in basket.Items)
            {
                var coupon = await _discountService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepository.UpdateBasket(basket));
        }

        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpDelete("{userName}")]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}
