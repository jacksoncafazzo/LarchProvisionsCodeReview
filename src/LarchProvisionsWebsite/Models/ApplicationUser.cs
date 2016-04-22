using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Order> Orders { get; set; }
        public ICollection<Menu> Menus { get; set; }

        public ApplicationUser()
        {
            Menus = new HashSet<Menu>();
            Orders = new HashSet<Order>();
        }
    }
}