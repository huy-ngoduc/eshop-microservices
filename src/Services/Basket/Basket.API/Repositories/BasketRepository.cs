using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basketCache = await _cache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basketCache))
            {
                return new ShoppingCart(userName);
            }

            var basket = JsonConvert.DeserializeObject<ShoppingCart>(basketCache);

            return basket ?? new ShoppingCart(userName);
        }

        public async Task UpdateBasket(ShoppingCart basket)
        {
            await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
        }

        public async Task DeleteBasket(string userName)
        {
            await _cache.RemoveAsync(userName);
        }
    }
}