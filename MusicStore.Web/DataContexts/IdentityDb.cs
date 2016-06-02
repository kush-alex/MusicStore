using Microsoft.AspNet.Identity.EntityFramework;
using MusicStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Web.DataContexts
{

    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static IdentityDb Create()
        {
            return new IdentityDb();
        }

    }
}
