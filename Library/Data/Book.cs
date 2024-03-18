using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Data
{
    [Comment("Book")]
    public class Book
    {
        [Comment("Primary key")]
        [Key]
        public int Id { get; set; }

        [Comment("Title of the book")]
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Comment("Author of the book")]
        [Required]
        [MaxLength(50)]
        public string Author { get; set; } = null!;

        [Comment("Description of the book")]
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = null!;

        [Comment("Url of the image")]
        [Required]
        public string ImageUrl { get; set; } = null!;

        [Comment("Rating of the book")]
        [Required]
        [Range(0.00, 10.00)]
        public decimal Rating { get; set; }

        [Comment("Id of the category of the book")]
        [Required]
        public int CategoryId { get; set; }

        [Comment("Category of the book")]
        [Required]
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Comment("Collection of books users")]
        public virtual ICollection<IdentityUserBook> UsersBooks { get; set; } = new List<IdentityUserBook>();
    }
}
