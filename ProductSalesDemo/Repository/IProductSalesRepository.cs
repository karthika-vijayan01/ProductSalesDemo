using Microsoft.AspNetCore.Mvc;
using ProductSalesDemo.Models;
using ProductSalesDemo.ViewModel;

namespace ProductSalesDemo.Repository
{
    public interface IProductSalesRepository
    {
        #region   1  - Get all orders from DB - Search All
        //Get all employees from DB - Search All
        public Task<ActionResult<IEnumerable<Order>>> GetAllOrders();
        #endregion

        #region  2  - Get all orders using ViewModel
        public Task<ActionResult<IEnumerable<ProductionSalesViewModel>>> GetOrderViewModel();
        #endregion

        #region 3 - Get an order based on Id
        public Task<ActionResult<Order>> GetOrderById(int id);
        #endregion

        #region  4  - Insert an order -return order record
        public Task<ActionResult<Order>> PostTblOrderReturnRecord(Order orderTable);
        #endregion

        #region    5 - Insert an order -return Id
        public Task<ActionResult<int>> PostTblOrderReturnId(Order orderTable);
        #endregion

        #region  6  - Update an order with ID 
        public Task<ActionResult<Order>> PutTblOrder(int id, Order orderTable);
        #endregion

        #region 7  - Delete an order
        public JsonResult DeleteTblOrder(int id); //return type > JsonResult -> true/false
        #endregion

        #region 8  - Get all orderItems
        public Task<ActionResult<IEnumerable<OrderItem>>> GetTblOrderItems();
        #endregion

        #region  9  - Get all Customer
        public Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers();
        #endregion

        #region  10  - Get all Brand
        public Task<ActionResult<IEnumerable<Brand>>> GetAllBrands();
        #endregion

        #region  11  - Get all Category
        public Task<ActionResult<IEnumerable<Category>>> GetAllCategoriess();
        #endregion

        #region  12  - Get all Manager
        public Task<ActionResult<IEnumerable<Manager>>> GetAllManagers();
        #endregion

        #region  13  - Get all Products
        public Task<ActionResult<IEnumerable<Product>>> GetAllProducts();
        #endregion

        #region  14  - Get all Staff
        public Task<ActionResult<IEnumerable<Staff>>> GetAllStaff();
        #endregion

        #region  15  - Get all Stock
        public Task<ActionResult<IEnumerable<Stock>>> GetAllStock();
        #endregion

        #region  16  - Get all Store
        public Task<ActionResult<IEnumerable<Store>>> GetAllStores();
        #endregion
    }
}
