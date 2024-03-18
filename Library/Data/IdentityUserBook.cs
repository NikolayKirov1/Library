using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Data
{
    [Comment("Identity user book")]
    public class IdentityUserBook
    {
        [Comment("Id of the collector")]
        [Required]
        public string CollectorId { get; set; } = null!;

        [Comment("Collector of the book")]
        [ForeignKey(nameof(CollectorId))]
        public IdentityUser Collector { get; set; } = null!;

        [Comment("Id of the book")]
        [Required]
        public int BookId { get; set; }

        [Comment("Book")]
        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;
    }
}
