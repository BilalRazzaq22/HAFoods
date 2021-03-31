using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel
{
    public class DBInstance
    {
        private static readonly object dbLock = new object();
        private static HAFoods_DBEntities _instance;

        public static HAFoods_DBEntities Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (dbLock)
                    {
                        if (_instance == null)
                            _instance = new HAFoods_DBEntities();
                    }
                }
                return _instance;
            }
        }

    }
}
