namespace ReadData.Models
{
    public class Category
    {
        public Category()
        {
            Products = new List<Product>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
    }
}
