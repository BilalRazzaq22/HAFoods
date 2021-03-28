using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Common
{
    public interface ICriteriaView
    {
        object Criteria { get; set; }
        void Load(object criteria);
    }
}
