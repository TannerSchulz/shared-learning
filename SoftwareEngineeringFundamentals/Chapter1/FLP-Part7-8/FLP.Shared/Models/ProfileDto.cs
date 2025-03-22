using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLP.Shared.Models
{
    public class ProfileDto
    {
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Biography { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
