﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsWebsite.Models
{
    public class Setting
    {
        public int SettingId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}