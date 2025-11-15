using API.Response;
using BLL.Services.AdminOrderService;
using DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IAdminOrderService _orderService;

        public OrdersController(IAdminOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllOrdersAsync(cancellationToken);
            return Ok(ResponseHelper.Success(orders));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(string id, CancellationToken cancellationToken)
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(id, cancellationToken);
            if (orderDetails == null)
            {
                return NotFound(ResponseHelper.Fail<object>("Order not found"));
            }
            return Ok(ResponseHelper.Success(orderDetails));
        }

        public class UpdateStatusRequest
        {
            public OrderStatus NewStatus { get; set; }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(id, request.NewStatus, cancellationToken);
                return Ok(ResponseHelper.Success("Status updated successfully"));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(ResponseHelper.Fail<object>("Order not found"));
            }
        }
    }
}