#nullable disable

using DataAccess.Enum;
using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
   public class User : RecordBase
    {

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public Prof Prof { get; set; }

        public DateTime BirthDate { get; set; }
        public bool IsAdmin{ get; set; }

        public String Email { get; set; }
        public String Password { get; set; }


        public List<Post> Posts { get; set; }

        public List<PostOwner> PostOwners { get; set; }

    }
}
