using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RJTECH_Authentication_.Models;
using System.Data;
using static Dapper.SqlMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RJTECH_Authentication_.Models
{
    public class OrderRepository
    {
        static string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True";
        IRepository<OrderDetail> _repository=new GenericRepository<OrderDetail>("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True");
        public void Add(OrderDetail o)
        {

            using (var connection = new SqlConnection(connect))
            {
                connection.Open();


                string query = "INSERT INTO OrderDetail (Id,PlacedAt,Name,Address,City,Country,PostalCode) VALUES (@Id,@PlacedAt,@Name,@Address,@City,@Country,@PostalCode);";
                connection.Execute(query, o);
            }
        }

        public OrderDetail getOrder(int id)
        {
           return _repository.GetById(id);
        }
        public List<CartItems> getOrderProducts(string id)
        { 
         List<CartItems> cartItems = new List<CartItems>();

            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                string query = "SELECT * FROM CartItems WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CartItems cartItem = new CartItems
                            {
                                UserId = reader["UserId"].ToString(),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Name = reader["Name"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Discprice = Convert.ToDecimal(reader["Discprice"]),
                                Image = reader["Image"].ToString(),
                                Category = reader["Category"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"])
                            };
                    cartItems.Add(cartItem);
                        }
                    }
                }
            }
            return cartItems;
        }
        public void AddOrderProduct(List<OrderProduct> product)
        {
            using (var connection = new SqlConnection(connect))
            {
                connection.Open();
                var query = $"INSERT INTO OrderProduct (OrderId,ProductId) VALUES (@OrderId,@ProductId);";
                connection.Execute(query, product);
            }
        }
        public void deleteCartItems(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connect))
            {
                conn.Open();
                string query = "delete FROM CartItems WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
