#nullable disable

using DataAccess.Enum;
using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class UserModel : RecordBase
    {
        #region Entity Properties

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public Prof Prof { get; set; }

        public DateTime BirthDate { get; set; }
        public bool IsAdmin { get; set; }

        public String Email { get; set; }
        public String Password { get; set; }

        #endregion


        #region Extra Properties
        [DisplayName("Posts")]
        public List<int> PostIdsInput { get; set; }

        [DisplayName("Birth Date")]
        public string BirthDateOutput { get; set; }


        [DisplayName("Email")]
        public string EmailOutput { get; set; }

        [DisplayName("Password")]
        public string PasswordOutput { get; set; }

        [DisplayName("Is Admin")]
        public string IsAdminOutput { get; set; }

        [DisplayName("Name")]
        public string FullNameOutput { get; set; }

        [DisplayName("Posts")]
        public string PostsNamesOutput { get; set; }

        #endregion

    }
}
