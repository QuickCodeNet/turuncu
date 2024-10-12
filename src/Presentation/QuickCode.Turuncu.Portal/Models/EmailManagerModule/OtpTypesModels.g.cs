using QuickCode.Turuncu.Common.Nswag.Clients.EmailManagerModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.Turuncu.Portal.Helpers;

namespace QuickCode.Turuncu.Portal.Models.EmailManagerModule
{
    public class OtpTypesData
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public OtpTypesObj SelectedItem { get; set; }

        public Dictionary<string, IEnumerable<SelectListItem>> ComboList = new Dictionary<string, IEnumerable<SelectListItem>>();
        public List<OtpTypesObj> List { get; set; }
    }

    public class OtpTypesObj : OtpTypesDto
    {
        private object[] KeyValues
        {
            get
            {
                return new object[]
                {
                    Id
                };
            }
        }

        public string _Key
        {
            get
            {
                return String.Join("|", this.KeyValues.Select(i => i.AsString()));
            }
        }
    }
}