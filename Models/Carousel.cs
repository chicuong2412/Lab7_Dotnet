using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab7_LeChiCuong_2131200001.Models
{
    public class Carousel
    {

        [Key]
        public int CarouselId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string ImageUrl { get; set; }


        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? LinkUrl { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate  { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }


    }
}
