using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lab7_LeChiCuong_2131200001.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        [JsonIgnore]
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
