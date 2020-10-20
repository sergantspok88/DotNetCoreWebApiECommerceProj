using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommwebapi.Data
{
    public interface IUnitOfWork
    {
        IProductRepo ProductRepo
        {
            get;
        }
        IUserRepo UserRepo
        {
            get;
        }
        int Complete();
    }
}
