namespace OrderService.models
{
    public class Order
    {
        public  int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public  decimal Price{ get; set; }
        public string Status { get; set; } = "Pending";
    }
}
