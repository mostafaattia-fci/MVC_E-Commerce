using API.Response;
using BLL.Services.Cart;
using DA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        private string GetUserId() =>
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

  
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetCartAsync(GetUserId());
            return Ok(ResponseHelper.Success(cart));
        }

      
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(string productId, int qty)
        {
            if (string.IsNullOrEmpty(productId) || qty <= 0)
                return BadRequest(ResponseHelper.Fail<string>("Invalid product or quantity"));

            await _cartService.AddItemToCartAsync(GetUserId(), productId, qty);
            return Ok(ResponseHelper.Success<string>("Item added to cart"));
        }

      
        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> Remove(string cartItemId)
        {
            if (string.IsNullOrEmpty(cartItemId))
                return BadRequest(ResponseHelper.Fail<string>("Cart item ID is required"));

            await _cartService.RemoveItemAsync(cartItemId);
            return Ok(ResponseHelper.Success<string>("Item removed"));
        }

    
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity(string cartItemId, int qty)
        {
            if (string.IsNullOrEmpty(cartItemId) || qty < 1)
                return BadRequest(ResponseHelper.Fail<string>("Invalid quantity"));

            await _cartService.UpdateQuantityAsync(cartItemId, qty);
            return Ok(ResponseHelper.Success<string>("Quantity updated"));
        }
    }
}
