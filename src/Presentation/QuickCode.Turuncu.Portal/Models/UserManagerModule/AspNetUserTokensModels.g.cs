using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.Turuncu.Portal.Helpers;

namespace QuickCode.Turuncu.Portal.Models.UserManagerModule
{
    public class AspNetUserTokensData
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecord { get; set; }
        public string SelectedKey { get; set; }
        public AspNetUserTokensObj SelectedItem { get; set; }

        public Dictionary<string, IEnumerable<SelectListItem>> ComboList = new Dictionary<string, IEnumerable<SelectListItem>>();
        public List<AspNetUserTokensObj> List { get; set; }
    }

    public class AspNetUserTokensObj : AspNetUserTokensDto
    {
        private object[] KeyValues
        {
            get
            {
                return new object[]
                {
                    UserId
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