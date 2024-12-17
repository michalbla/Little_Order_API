using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNAI.Model.Entities;

namespace TNAI.Repository.Orders
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetAllOrdersAsync();
        Task<bool> SaveOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
    }
}
