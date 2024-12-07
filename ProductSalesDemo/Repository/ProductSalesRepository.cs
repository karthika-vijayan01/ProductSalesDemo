using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSalesDemo.Models;
using ProductSalesDemo.Repository;
using ProductSalesDemo.ViewModel;

namespace ProductSalesDemo.Repository
{
    public class ProductSalesRepository : IProductSalesRepository
    {
        private readonly ProductSalesAssContext _context;

        public ProductSalesRepository(ProductSalesAssContext context)
        {
            _context = context;
        }

        #region  1  - Get all orders from DB - Search All
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Orders.Include(order => order.Customer).Include(order => order.OrderItems)
                        .Include(order => order.Staff).Include(order => order.Store).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Order>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  2 - Order ViewModel
        public async Task<ActionResult<IEnumerable<ProductionSalesViewModel>>> GetOrderViewModel()
        {
            //LINQ
            try
            {
                if (_context != null)
                {
                    //LINQ
                    return await (from o in _context.Orders
                                  join c in _context.Customers on o.CustomerId equals c.CustomerId
                                  join oi in _context.OrderItems on o.OrderId equals oi.OrderId
                                  join s in _context.Stores on o.StoreId equals s.StoreId
                                  select new ProductionSalesViewModel
                                  {
                                      CustomerId = c.CustomerId,
                                      FirstName = c.FirstName,
                                      Phone = c.Phone,
                                      Email = c.Email,
                                      OrderDate = o.OrderDate,
                                      RequiredDate = o.RequiredDate,
                                      ShippedDate = o.ShippedDate,
                                      Quantity = oi.Quantity,
                                      Discount = oi.Discount,
                                      StoreName = s.StoreName
                                  }).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<ProductionSalesViewModel>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   3 - Get an order based on Id
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                if (_context != null)
                {
                    var custOrder = await _context.Orders.Include(order => order.Customer).Include(order => order.OrderItems)
                        .Include(order => order.Staff).Include(order => order.Store).
                        FirstOrDefaultAsync(ord => ord.OrderId == id);
                    return custOrder;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   4  - Insert an order -return order record
        public async Task<ActionResult<Order>> PostTblOrderReturnRecord(Order orderTable)
        {
            try
            {
                if (orderTable == null)
                {
                    throw new ArgumentException(nameof(orderTable), "Order data is null");
                    //return null;
                }
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized");
                }
                await _context.Orders.AddAsync(orderTable);
                await _context.SaveChangesAsync();
                var OrderCustOI = await _context.Orders.Include(order => order.Customer).Include(order => order.OrderItems)
                    .Include(order => order.Staff).Include(order => order.Store)
                    .FirstOrDefaultAsync(ord => ord.OrderId == orderTable.OrderId);

                return OrderCustOI;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   5 - Insert an order -return Id
        public async Task<ActionResult<int>> PostTblOrderReturnId(Order orderTable)
        {
            try
            {
                if (orderTable == null)
                {
                    throw new ArgumentException(nameof(orderTable), "Order data is null");
                }
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized");
                }
                await _context.Orders.AddAsync(orderTable);
                var changesRecord = await _context.SaveChangesAsync();
                if (changesRecord > 0)
                {
                    return orderTable.OrderId;
                }
                else
                {
                    throw new Exception("Failed to save Order record to the database");
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  6  - Update an order with ID 
        public async Task<ActionResult<Order>> PutTblOrder(int id, Order orderTable)
        {
            try
            {
                if (orderTable == null)
                {
                    throw new InvalidOperationException("Database context is not initialized");
                }
                //Find the employee by id
                var existingOrder = await _context.Orders.FindAsync(id);
                if (existingOrder == null)
                {
                    return null;
                }

                //Map values wit fields
                existingOrder.OrderDate = orderTable.OrderDate;
                existingOrder.RequiredDate = orderTable.RequiredDate;
                existingOrder.ShippedDate = orderTable.ShippedDate;

                //save changes to the database
                await _context.SaveChangesAsync();

                //Retreive the employee with the related Department
                var OrderCustomerOI = await _context.Orders.Include(order => order.Customer).Include(order => order.OrderItems)
                    .Include(order => order.Staff).Include(order => order.Store)
                    .FirstOrDefaultAsync(existingOrder => existingOrder.OrderId == orderTable.OrderId);

                //Return the added employee record
                return OrderCustomerOI;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  7  - Delete an order
        public JsonResult DeleteTblOrder(int id)
        {
            try
            {
                if (id <= null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Invalid Order Id"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                //Ensure the context is not null
                if (_context == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Database context is not initialized"
                    })
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }

                //Find the employee by id
                var existingOrder = _context.Orders.Find(id);

                if (existingOrder == null)
                {
                    return new JsonResult(new
                    {
                        success = false,
                        message = "Order not found"
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                //Remove the employee record from the DBContext
                _context.Orders.Remove(existingOrder);

                //save changes to the database
                _context.SaveChangesAsync();
                return new JsonResult(new
                {
                    success = true,
                    message = "Order Deleted successfully"
                })
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Database context is not initialized"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        #endregion

        #region   8  - Get all orderItems
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetTblOrderItems()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.OrderItems.Include(order => order.Product).Include(order => order.Order).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<OrderItem>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  9  - Get all Customer
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Customers.Include(order => order.Orders).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Customer>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  10  - Get all Brand
        public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Brands.Include(order => order.Products).ToListAsync();

                }
                //Returns an empty list if context is null
                return new List<Brand>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  11  - Get all Category
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriess()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Categories.Include(order => order.Products).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Category>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   12  - Get all Manager
        public async Task<ActionResult<IEnumerable<Manager>>> GetAllManagers()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Managers.Include(order => order.Stores).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Manager>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   13  - Get all Products
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Products.Include(order => order.Brand).Include(order => order.Category).
                        Include(order => order.OrderItems).Include(order => order.Stocks).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Product>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   14  - Get all Staff
        public async Task<ActionResult<IEnumerable<Staff>>> GetAllStaff()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Staffs.Include(order => order.Store).Include(order => order.Orders).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Staff>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region  15  - Get all Stock
        public async Task<ActionResult<IEnumerable<Stock>>> GetAllStock()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Stocks.Include(order => order.Store).Include(order => order.Product).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Stock>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region   16  - Get all Store
        public async Task<ActionResult<IEnumerable<Store>>> GetAllStores()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.Stores.Include(order => order.Manager).Include(order => order.Staff)
                        .Include(order => order.Orders).Include(order => order.Stocks).ToListAsync();
                }
                //Returns an empty list if context is null
                return new List<Store>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}
