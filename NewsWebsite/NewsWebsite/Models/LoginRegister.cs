﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.Models
{
    public class LoginRegister
    {
        public NewsWebsite.Models.RegisterViewModel Item1 { get; set; }
        public NewsWebsite.Models.LoginViewModel Item2 { get; set; }
        public NewsWebsite.Models.ExternalLoginConfirmationViewModel Item3 { get; set; }
        public NewsWebsite.Models.IndexViewModel Item4 { get; set; }
    }
}