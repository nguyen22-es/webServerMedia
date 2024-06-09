

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Common.Entities
{
    public class VODUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string? Token { get; set; }
        public DateTime TokenExpires { get; set; }

        [NotMapped]
        public IList<Claim> Claims { get; set; } = new List<Claim>();
    }
}
