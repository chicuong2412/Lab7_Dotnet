using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lab7_LeChiCuong_2131200001.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab7_LeChiCuong_2131200001.Models
{
    [PrimaryKey("UserId", "RoleId")]
    public class UserRole
    {
        
        public int UserId { get; set; }

        [ForeignKey("UserId")]

        [JsonIgnore]
        public User User { get; set; }

        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
