using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _disDiscountGrpcService;
        public BasketController(IBasketRepository repository, DiscountGrpcService disDiscountGrpcService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _disDiscountGrpcService =
                disDiscountGrpcService ?? throw new ArgumentNullException(nameof(disDiscountGrpcService));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket); // tao basket lan dau
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //TODO: giao tiếp với Discount.Grpc sau đó tính các giá cuối cùng của product trong shopping cart
            foreach (var item in basket.Items)
            {
                var coupon = await _disDiscountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }

            await _repository.UpdateBasket(basket);
            return Ok();
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
