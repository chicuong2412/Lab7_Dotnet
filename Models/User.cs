using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lab7_LeChiCuong_2131200001.Models;

namespace Lab7_LeChiCuong_2131200001.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Fullname is required.")]
        [Column(TypeName = "nvarchar(200)")]
        public string Fullname { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Column(TypeName = "nvarchar(200)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Column(TypeName = "nvarchar(max)")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Column(TypeName = "nvarchar(20)")]
        public string Phone { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "User code is required.")]
        [Column(TypeName = "nvarchar(max)")]
        public string UserCode { get; set; }

        public bool IsLocked { get; set; } = false;

        public bool IsDeleted { get; set; } = false;

        public bool IsActive { get; set; } = false;

        [Column(TypeName = "nvarchar(max)")]
        public string ActiveCode { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string Avatar { get; set; } = string.Empty;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        [NotMapped]
        public List<int> SelectedRoleIds { get; set; }

    }
}
