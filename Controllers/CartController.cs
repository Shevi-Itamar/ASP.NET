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
    var cart = CartService.GetCart(userId);
    if (cart == null)
    {
        return NotFound("Cart is empty");
    }

    return Ok(cart);
}

[HttpPost("{userId}")]
[Authorize(policy: "UserOrAdmin")]
public IActionResult AddToCart(int userId, [FromBody] JewelRequest request)
{
    var jewel = JewelService.Get(request.ID);
    if (jewel == null)
    {
        return NotFound("Jewel not found");
    }

    CartService.AddToCart(userId, jewel.Id);
    return Ok("Item added to cart");
}

[HttpDelete("{userId}/{jewelId}")]
[Authorize(policy: "UserOrAdmin")]
public IActionResult RemoveFromCart(int userId, int jewelId)
{
    CartService.RemoveFromCart(userId, jewelId);
    return Ok("Item removed from cart");
}

}
