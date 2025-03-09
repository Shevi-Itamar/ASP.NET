using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using lessson1.Models;
using lessson1.Interfaces;

namespace lessson1.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private ICartService CartService;
    private IJewelService JewelService;

    public CartController(ICartService cartService, IJewelService jewelService)
    {
        this.CartService = cartService;
        this.JewelService = jewelService;
    }

   [HttpGet("{userId}")]
[Authorize(policy: "UserOrAdmin")]
public IActionResult GetCart(int userId)
{
    Console.WriteLine($"[CartController] GetCart called for userId: {userId}");  // הדפסה להתחלת הקריאה
    var cart = CartService.GetCart(userId);
    if (cart == null)
    {
        Console.WriteLine($"[CartController] Cart not found for userId: {userId}");  // הדפסה אם העגלה לא נמצאה
        return NotFound("Cart is empty");
    }

    Console.WriteLine($"[CartController] Cart found for userId: {userId}, cart items count: {cart.Count}");  // הדפסה אם נמצאה עגלה
    return Ok(cart);
}

[HttpPost("{userId}")]
[Authorize(policy: "UserOrAdmin")]
public IActionResult AddToCart(int userId, [FromBody] JewelRequest request)
{
    Console.WriteLine($"[CartController] AddToCart called for userId: {userId}, jewelId: {request.ID}");  // הדפסה להתחלת הקריאה
    var jewel = JewelService.Get(request.ID);
    if (jewel == null)
    {
        Console.WriteLine($"[CartController] Jewel not found for jewelId: {request.ID}");  // הדפסה אם התכשיט לא נמצא
        return NotFound("Jewel not found");
    }

    CartService.AddToCart(userId, jewel.Id);
    Console.WriteLine($"[CartController] Jewel added to cart for userId: {userId}");  // הדפסה אחרי הוספת התכשיט
    return Ok("Item added to cart");
}

[HttpDelete("{userId}/{jewelId}")]
[Authorize(policy: "UserOrAdmin")]
public IActionResult RemoveFromCart(int userId, int jewelId)
{
    Console.WriteLine($"[CartController] RemoveFromCart called for userId: {userId}, jewelId: {jewelId}");  // הדפסה להתחלת הקריאה
    CartService.RemoveFromCart(userId, jewelId);
    Console.WriteLine($"[CartController] Jewel removed from cart for userId: {userId}, jewelId: {jewelId}");  // הדפסה אחרי מחיקת התכשיט
    return Ok("Item removed from cart");
}

}
