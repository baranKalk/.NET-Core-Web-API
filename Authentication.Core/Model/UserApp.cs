﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Model
{
	public class UserApp:IdentityUser
	{
        public string City { get; set; }

    }
}
