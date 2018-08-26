using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Models
{
    public class TestUser : IdentityUser<int>
    {
        public int Age { get; set; }
    }
}
