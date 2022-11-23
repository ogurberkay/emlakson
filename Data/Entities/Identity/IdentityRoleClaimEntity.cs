using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Identity
{
    public class IdentityRoleClaimEntity : IdentityRoleClaim<int>
    {
        public virtual IdentityRoleEntity Role { get; set; }
    }
}
