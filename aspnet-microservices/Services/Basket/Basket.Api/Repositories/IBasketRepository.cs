using Basket.Api.Entities;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetUserBasket(string username);

        Task<ShoppingCart> UpdateUserBasket(ShoppingCart basket);

        Task DeleteUserBasket(string username);
    }
}
