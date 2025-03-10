using lessson1.Interfaces;
using lessson1.Models;
using Newtonsoft.Json;

namespace lessson1.Services
{
    public class CartService : ICartService
    {
        private readonly IUserService _userService;
        private readonly IJewelService _jewelService;

        public CartService(IUserService userService, IJewelService jewelService)
        {
            _userService = userService;
            _jewelService = jewelService;
        }

     public List<Jewel> GetCart(int userId)
{
    var user = _userService.Get(userId);
    if (user == null)
    {
        return new List<Jewel>();
    }

    return user?.Cart ?? new List<Jewel>();
}

public void AddToCart(int userId, int jewelId)
{
    var user = _userService.Get(userId);
    if (user == null)
    {
        return;
    }

    var jewel = _jewelService.Get(jewelId);
    if (jewel == null)
    {
        return;
    }

    user.Cart ??= new List<Jewel>();
    user.Cart.Add(jewel);
    _userService.Update(user, false);

}

public void RemoveFromCart(int userId, int jewelId)
{
    var user = _userService.Get(userId);
    if (user == null || user.Cart == null)
    {
        return;
    }

    user.Cart.RemoveAll(j => j.Id == jewelId);
    _userService.Update(user, false);

}
}

     public static class CartServiceHelper
    {
        public static void AddCartService(this IServiceCollection BuilderService)
        {
            BuilderService.AddSingleton<ICartService,CartService>();
        }
    }

}
