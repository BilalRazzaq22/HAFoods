using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common
{
    public interface ILoadable
    {
        void LoadRecord(object primaryKey);
    }
}
