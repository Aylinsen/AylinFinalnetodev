#nullable disable

using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CategoryModel : RecordBase
    {
        #region Entity Properties

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "{0} must be minimum {2} and maximum {1} characters!")]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }

        #endregion

        #region Extra Properties

        [DisplayName("Book Count")]
        public int PostCountOutput { get; set; }


        [DisplayName("Book Names")]
        public string PostNamesOutput { get; set; }
        #endregion

    }
}
