using Library.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class AllBooksViewModel
    {
        public string ImageUrl { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string Rating { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int Id { get; set; }
    }
}
