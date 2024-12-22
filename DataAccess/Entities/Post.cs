#nullable disable
using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Post : RecordBase
    {
        [Required]
        [StringLength(40)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime PostedOn { get; set; }

        [Required]
        public int UserId { get; set; } 
        public User User { get; set; }

        [Required]
        public int CategoryId { get; set; } 
        public Category Category { get; set; }

        public List<PostOwner> PostOwners { get; set; }
    }
}
