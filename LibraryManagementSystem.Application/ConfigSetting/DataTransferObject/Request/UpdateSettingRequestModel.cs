using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.ConfigSetting.DataTransferObject.Request
{
    public class UpdateSettingRequestModel
    {
        public int? loanPeriodDays { get; set; } 
        public decimal? fineRatePerDay { get; set; }
        public int? maxActiveLoans { get; set; } 
        public decimal? UnpaidFinethreshold { get; set; }
    }
}
