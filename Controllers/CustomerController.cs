using ABC_Retail.Models;
using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABC_Retail.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableService _tableService;

        public CustomersController(TableService tableService)
        {
            _tableService = tableService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetAllCustomersAsync();
            return View(customers);
        }

        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return View(customer);
            }

            customer.PartitionKey = "CustomersPartition";
            customer.RowKey = Guid.NewGuid().ToString();

            await _tableService.AddCustomerAsync(customer);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCustomer(string partitionKey, string rowKey)
        {
            await _tableService.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var customer = await _tableService.GetCustomerAsync(partitionKey, rowKey);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
    }
}
