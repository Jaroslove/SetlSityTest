using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetlSityTest.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}