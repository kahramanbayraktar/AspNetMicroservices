using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        { 
            return Ok(await _basketRepository.UpdateBasket(basket));
        }

        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasket(userName);
            return Ok();
        }
    }
}
