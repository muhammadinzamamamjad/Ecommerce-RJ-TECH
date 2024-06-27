using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Core.Types;
using System.Formats.Tar;

namespace RJTECH_Authentication_.Models
{
    public class CartRepository
    {

        private readonly string _connectionString;
        string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True";
        IRepository<CartItems> _repository = new GenericRepository<CartItems>("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True");
        public CartRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CartItems GetItem(int productId, string userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"
                SELECT * FROM CartItems
                WHERE ProductId = @ProductId AND UserId = @UserId";
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@UserId", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new CartItems
                        {
                            ProductId = (int)reader["ProductId"],
                            UserId = (string)reader["UserId"],
                            Name = (string)reader["Name"],
                            Price = (decimal)reader["Price"],
                            Discprice = (decimal)reader["Discprice"],
                            Image = (string)reader["Image"],
                            Category = (string)reader["Category"],
                            Quantity = (int)reader["Quantity"]
                        };
                    }
                    else
                    {
                        return null; // Cart item not found
                    }
                }
            }
        }
        public void Add(CartItems cartItem)
        {
            _repository.Add(cartItem);
        }
        public void Update(CartItems cartItem)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"
                UPDATE CartItems
                SET Quantity = @Quantity
                WHERE ProductId = @ProductId AND UserId = @UserId";
                command.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                command.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                command.Parameters.AddWithValue("@UserId", cartItem.UserId);

                command.ExecuteNonQuery();
            }
        }
        public List<CartItems> GetItemsByUser(string userId)
        {
            List<CartItems> cartItems = new List<CartItems>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM CartItems WHERE UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

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
        public void Remove(CartItems cartItem)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM CartItems WHERE ProductId = @ProductId AND UserId = @UserId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                    cmd.Parameters.AddWithValue("@UserId", cartItem.UserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


