using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuickCode.Turuncu.Portal.Models
{
    public class ComboBoxFormatData
    {
        public string ValueField { get; set; }
        public string StringFormat { get; set; }
        public string[] TextFields { get; set; }
    }

}

