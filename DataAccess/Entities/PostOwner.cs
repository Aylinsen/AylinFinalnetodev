using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace DataAccess.Entities
{
    public class PostOwner: RecordBase
    {

      
        public int PostId { get; set; }
        public Post Post { get; set; }

    }
}
