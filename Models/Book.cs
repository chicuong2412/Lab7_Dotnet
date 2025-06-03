using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab7_LeChiCuong_2131200001.Models
{
    public class Book
    {

        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "BookCode is required.")]
        [Column(TypeName = "nvarchar(max)")]
        public string BookCode { get; set; }

        [Required(ErrorMessage = "Publisher is required.")]
        [Column(TypeName = "nvarchar(max)")]
        public string Publisher { get; set; }

        public int TotalCopies { get; set; } = 0;

        public int AvailableCopies { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Column(TypeName = "nvarchar(max)")]
        public string? Avatar { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? Pdf { get; set; } = string.Empty;

        
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        public ICollection<Category> Categories { get; set; } = new List<Category>();

        public ICollection<Author> Authors { get; set; } = new List<Author>();

        [NotMapped]
        public List<int>? SelectedCategoryIds { get; set; }

        [NotMapped]
        public List<int>? SelectedAuthorIds { get; set; }

    }
}
