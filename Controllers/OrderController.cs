using ABC_Retail.Models;
using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Retail.Controllers
{
    public class OrdersController : Controller
    {
        private readonly TableService _tableService;
        private readonly QueueService _queueService;

        public OrdersController(TableService tableService, QueueService queueService)
        {
            _tableService = tableService;
            _queueService = queueService;
        }

        // Action to display all orders (optional first)
        public async Task<IActionResult> Index()
        {
            var orders = await _tableService.GetAllOrdersAsync();
            return View(orders);
        }
        public async Task<IActionResult> Register()
        {
            var customers = await _tableService.GetAllCustomersAsync();
            var products = await _tableService.GetAllProductsAsync();

            // Check for null or empty lists
            if (customers == null || customers.Count == 0)
            {
                // Handle the case where no customers are found
                ModelState.AddModelError("", "No customers found. Please add customers first.");
                return View(); // Or redirect to another action
            }

            if (products == null || products.Count == 0)
            {
                // Handle the case where no products are found
                ModelState.AddModelError("", "No products found. Please add products first.");
                return View(); // Or redirect to another action
            }

            ViewData["Customers"] = customers;
            ViewData["Products"] = products;

            return View();
        }



        // Action to handle the form submission and register the order
        [HttpPost]
        public async Task<IActionResult> Register(Order order)
        {
            if (ModelState.IsValid)
            {//TableService
                order.Date = DateTime.SpecifyKind(order.Date, DateTimeKind.Utc);
                order.PartitionKey = "OrdersPartition";
                order.RowKey = Guid.NewGuid().ToString();
                await _tableService.AddOrderAsync(order);
                //MessageQueue
                string message = $"New order made by customer {order.Customer_Id} of product {order.Product_Id} at {order.Shipping_Address} on {order.Date}";
                await _queueService.SendMessageAsync(message);

                return RedirectToAction("Index");
            }
            else
            {
                // Log model state errors
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            // Reload customers and products lists if validation fails
            var customers = await _tableService.GetAllCustomersAsync();
            var products = await _tableService.GetAllProductsAsync();
            ViewData["Customers"] = customers;
            ViewData["Products"] = products;

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(string partitionKey, string rowKey)
        {
            await _tableService.DeleteOrderAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }
    }
}
