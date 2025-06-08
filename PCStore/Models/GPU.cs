namespace PCStore.Models
{
    public class GPU
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Model { get; set; }
        public decimal Price { get; set; }
        public int Memory { get; set; } // in GB
    }
}
