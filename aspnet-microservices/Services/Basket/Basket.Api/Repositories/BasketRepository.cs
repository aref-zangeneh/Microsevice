using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }


        public async Task<ShoppingCart> GetUserBasket(string username)
        {
            var basket = await _redisCache.GetStringAsync(username);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateUserBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.Username, JsonConvert.SerializeObject(basket));

            return await GetUserBasket(username: basket.Username);
        }

        public async Task DeleteUserBasket(string username)
        {
            await _redisCache.RemoveAsync(username);
        }
    }
}
