namespace Library.Models
{
    public class MyBooksViewModel
    {
        public string ImageUrl { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int Id { get; set; }
    }
}
