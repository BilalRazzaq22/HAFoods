using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Entities.DBModel.AppSettings
{
    public class AppSetting
    {
        public int Id { get; set; }
        public string AppVersion { get; set; }
        public bool IsDBCreated { get; set; }
        public DateTime? AppStartDate { get; set; }
        public DateTime? AppEndDate { get; set; }
    }
}
