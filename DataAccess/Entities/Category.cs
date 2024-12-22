#nullable disable

using DataAccess.Records.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Category : RecordBase
    {
        public string CategoryName { get; set; }

        public List<Post> Posts { get; set; }

    }
}
