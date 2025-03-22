using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLP.Data.Entities
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string? DisplayName { get; set; }
        public string? Biography { get; set; }
        public string? Website { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
