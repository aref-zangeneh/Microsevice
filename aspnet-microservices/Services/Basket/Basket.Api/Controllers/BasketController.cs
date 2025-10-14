using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        #region Ctor and Fields

        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcService _discountService;
        public BasketController(IBasketRepository basketRepository, DiscountGrpcService discountService)
        {
            _basketRepository = basketRepository;
            _discountService = discountService;
        }

        #endregion

        #region Methods
        [HttpGet("{username}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string username)
        {
            var basket = await _basketRepository.GetUserBasket(username);

            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // Todo: get data from discount gRPC and calculate latest prices of product into shopping cart
            foreach (var item in basket.Items)
            {
                var coupon = await _discountService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }


            return Ok(await _basketRepository.UpdateUserBasket(basket));
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string username)
        {
            await _basketRepository.DeleteUserBasket(username);
            return Ok();
        }
        #endregion
    }
}
