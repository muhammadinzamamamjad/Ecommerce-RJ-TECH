namespace RJTECH_Authentication_.Models
{
    public class CustomerOrder
    {
        public OrderDetail OrderDetail { get; set; }
        public List<CartItems> Products { get; set; }
    }
}
