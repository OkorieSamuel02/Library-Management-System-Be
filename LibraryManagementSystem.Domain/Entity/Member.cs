using LibraryManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Entity
{
    public class Member
    {
        public Guid id { get; set; }
        public string name { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;      
        public string contactNumber { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public DateTime membershipDate { get; set; } = DateTime.UtcNow;
        public MemberStatus Status {  get; private set; } = Enums.MemberStatus.Active;
        public ICollection<Loan>? loans { get; set; } = new List<Loan>();


        public void MemberStatus(bool value)
        {
            if(!value)
            {
                Status = Enums.MemberStatus.Suspended;
            }
            else
            {
                Status = Enums.MemberStatus.Active;
            }
        }
    }
}
