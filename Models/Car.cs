namespace TestDealershipApi.Models
{
    
    public class Car
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int ReleaseYear { get; set; }
        public string? Color { get; set; }
        public decimal Price { get; set; }
        public string? Condition { get; set; }
    }
}
