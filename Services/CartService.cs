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
    Console.WriteLine($"[CartService] GetCart called for userId: {userId}");  // הדפסה להתחלת הקריאה
    var user = _userService.Get(userId);
    if (user == null)
    {
        Console.WriteLine($"[CartService] User not found for userId: {userId}");  // הדפסה אם המשתמש לא נמצא
        return new List<Jewel>();
    }

    Console.WriteLine($"[CartService] Cart found for userId: {userId}, Cart count: {user.Cart?.Count}");  // הדפסה אם נמצא סל
    return user?.Cart ?? new List<Jewel>();
}

public void AddToCart(int userId, int jewelId)
{
    Console.WriteLine($"[CartService] AddToCart called for userId: {userId}, jewelId: {jewelId}");  // הדפסה להתחלת הקריאה
    var user = _userService.Get(userId);
    if (user == null)
    {
        Console.WriteLine($"[CartService] User not found for userId: {userId}");  // הדפסה אם המשתמש לא נמצא
        return;
    }

    var jewel = _jewelService.Get(jewelId);
    if (jewel == null)
    {
        Console.WriteLine($"[CartService] Jewel not found for jewelId: {jewelId}");  // הדפסה אם התכשיט לא נמצא
        return;
    }

    user.Cart ??= new List<Jewel>();
    user.Cart.Add(jewel);
    _userService.Update(user, false);

    Console.WriteLine($"[CartService] Jewel added to cart for userId: {userId}. New cart count: {user.Cart.Count}");  // הדפסה אחרי הוספת התכשיט
}

public void RemoveFromCart(int userId, int jewelId)
{
    Console.WriteLine($"[CartService] RemoveFromCart called for userId: {userId}, jewelId: {jewelId}");  // הדפסה להתחלת הקריאה
    var user = _userService.Get(userId);
    if (user == null || user.Cart == null)
    {
        Console.WriteLine($"[CartService] User or cart not found for userId: {userId}");  // הדפסה אם המשתמש או העגלה לא נמצאו
        return;
    }

    user.Cart.RemoveAll(j => j.Id == jewelId);
    _userService.Update(user, false);

    Console.WriteLine($"[CartService] Jewel removed from cart for userId: {userId}. New cart count: {user.Cart.Count}");  // הדפסה אחרי מחיקת התכשיט
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
