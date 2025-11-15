using BLL.Services.AdminOrderService;
using DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IAdminOrderService _orderService;

        public OrdersController(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: /Admin/Orders
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllOrdersAsync(cancellationToken);
            return View(orders);
        }

        // GET: /Admin/Orders/Details/{id}
        public async Task<IActionResult> Details(string id, CancellationToken cancellationToken)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(id, cancellationToken);
            if (orderDetails == null)
            {
                return NotFound();
            }
            return View(orderDetails);
        }

        // POST: /Admin/Orders/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(string orderId, OrderStatus newStatus, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest();
            }

            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, newStatus, cancellationToken);
                TempData["SuccessMessage"] = "Order status updated successfully.";
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
            }

            // العودة لنفس صفحة التفاصيل
            return RedirectToAction(nameof(Details), new { id = orderId });
        }
    }
}