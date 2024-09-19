﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AIMS.Infrastructure.IdentityClass
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(250)]
        public string FullName { get; set; }
        [MaxLength(250)]
        public string JobTitle { get; set; }

    }

    public class ApplicationRole : IdentityRole
    {
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
