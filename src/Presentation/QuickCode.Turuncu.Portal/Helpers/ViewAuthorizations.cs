using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QuickCode.Turuncu.Portal.Helpers;

namespace QuickCode.Turuncu.Portal.Helpers
{
    public class ViewPermission
    {
        public bool IsPageAvailable { get; set; }
        public bool Insert { get; set; }
        public bool Delete { get; set; }
        public bool Update { get; set; }
        public bool Detail { get; set; }
        public bool List { get; set; }
    }
    
    public class ViewPermissionItemData
    {
        public ViewPermission Item { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ItemId { get; set; }
    }
}