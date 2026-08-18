using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Entity
{
    public class Setting
    {
        public int id { get; set; }
        public int loanPeriodDays { get; set; } = 14;
        public decimal fineRatePerDay { get; set; } = 0.5m;
        public int maxActiveLoans { get; set; } = 3;
        public decimal UnpaidFinethreshold { get; set; }
    }
}
