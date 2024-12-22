using DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Bases
{
    public abstract class ServiceBase
    {
        protected readonly Db _db;

        protected ServiceBase(Db db)
        {
            _db = db;
        }
    }
}
