using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickCode.Turuncu.Portal.Helpers;

namespace QuickCode.Turuncu.Portal.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> GetEnumList<T>(int selectedValue = 0) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Type given must be an Enum");
            }

            return (from Enum item in Enum.GetValues(typeof(T))
                    select new SelectListItem
                    {  
                        Text = item.Description(),
                        Value = item.ToString()
                    }
                    );
            
          
        }

    }
}