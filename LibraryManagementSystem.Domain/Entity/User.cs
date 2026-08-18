using LibraryManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Domain.Entity
{
    public class User : IdentityUser
    {
        public string firstName {  get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string contactNumber {  get; set; } = string.Empty;
        public Roles Roles { get; set; }
    }
}
