using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    [Comment("Category of the book")]
    public class Category
    {
        [Comment("Primary key")]
        [Key]
        public int Id { get; set; }

        [Comment("Name of the category")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Comment("Collection of books")]
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
