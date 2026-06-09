namespace TestDealershipApi.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string? FCs { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly PurchaseDate { get; set; }
    }
}
