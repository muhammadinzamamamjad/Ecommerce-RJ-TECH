using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlClient;
namespace RJTECH_Authentication_.Models
{
    public class ProductRepository
    {
        string connect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True";
        IRepository<Product> _repository=new GenericRepository<Product>("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RJTECHDB;Integrated Security=True");
        List<Product> products = new List<Product>();
        public void Add(Product product)
        {

            _repository.Add(product);
        }
        public Product detail(int ID)
        {
            Product product = null;
            product=_repository.GetById(ID);
            return product;
        }

        public Product Edit(int ID)
        {
            Product product = null;
            product = _repository.GetById(ID);
            return product;
        }
        public void Update(Product product)
        {
           _repository.Update(product);

        }
        public void Remove(int ID)
        {
            _repository.Delete(ID);

        }
        public List<Product> get(string categry)
        {
            List<Product> products = new List<Product>();
            SqlConnection connection = new SqlConnection(connect);
            connection.Open();
            string query = $"select * from Product where category = @Categorey";

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("Categorey", categry);
            SqlDataReader reaader = cmd.ExecuteReader();
            while (reaader.Read())
            {
                Product product = new Product();
                product.Id = Convert.ToInt32(reaader[0]);
                product.Name = reaader["Name"].ToString();
                product.Price = Convert.ToDecimal(reaader["Price"]);
                product.Quantity = Convert.ToInt32(reaader["Quantity"]);
                product.Category = reaader["Category"].ToString();
                product.Images = reaader["Images"].ToString();

                products.Add(product);
            }
            return products;
        }
        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }
        public Product GetItem(int ID)
        {
            Product product = null;
            product = _repository.GetById(ID);
            return product;
        }
        public int GetStockQuantity(int productId)
        {
            int stockQuantity = 0;

            string query = "SELECT Quantity FROM Product WHERE Id = @ProductId";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    stockQuantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                }
            }

            return stockQuantity;
        }
        public void UpdateStockQuantity(int productId, int newQuantity)
        {
            string query = "UPDATE Product SET Quantity = @NewQuantity WHERE Id = @ProductId";

            using (SqlConnection connection = new SqlConnection(connect))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@NewQuantity", newQuantity);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
