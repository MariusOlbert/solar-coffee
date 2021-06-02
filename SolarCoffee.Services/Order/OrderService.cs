using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        public OrderService(SolarDbContext dbContext, ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _productService = productService;
            _inventoryService = inventoryService;
            _logger = logger; 
            _db = dbContext;
        }
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            _logger.LogInformation("Generating new sales oders");
            foreach(var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);
                item.Quantity = item.Quantity;
                var inventoryId = _inventoryService.GetByProductId(item.Product.Id).Id;

                _inventoryService.UpdateUnitsAvailable(inventoryId, -item.Quantity);
            }

            try
            {
                _db.SalesOrders.Add(order);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Data = true,
                    Message = "Open order created",
                    Time = DateTime.UtcNow
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Failed to created open order. Error: " + e.StackTrace,
                    Time = DateTime.UtcNow
                };
            }
        }

        public List<SalesOrder> GetOrders()
        {
            return _db.SalesOrders
                .Include(so => so.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                .ToList();                
        }

        // Marks open order is paid
        public ServiceResponse<bool> MarkFufilled(int id)
        {
            var order = _db.SalesOrders.Find(id);
            if (order != null)
            {
                order.UpdatedOn = DateTime.UtcNow;
                order.IsPaid = true;
                try
                {
                    _db.SalesOrders.Update(order);
                    _db.SaveChanges();
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = true,
                        Data = true,
                        Message = "Order closed. Invoice paid in full for order with ID: " + order.Id,
                        Time = DateTime.UtcNow
                    };
                }
                catch (Exception e)
                {
                    return new ServiceResponse<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "Order failed to update.",
                        Time = DateTime.UtcNow
                    };
                }
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Can't find order with this ID: " + order.Id,
                    Time = DateTime.UtcNow
                };
            }
        }
    }
}
