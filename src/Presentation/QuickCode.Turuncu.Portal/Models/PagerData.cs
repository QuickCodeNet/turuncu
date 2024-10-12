using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuickCode.Turuncu.Portal.Models
{

    public class UserDetail
    {
        public string NameSurname { get; set; }
        public string ImageUrl { get; set; }
    }

    public class BasicTestModel
    {
        public int NValue { get; set; }

         public string SValue { get; set; }

        public Dictionary<string, IEnumerable<SelectListItem>> ComboList = new Dictionary<string, IEnumerable<SelectListItem>>();
    }

    public class PagerData
    {
        public int TotalPage { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int NumberOfRecord { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }

    public class TextAreaData
    {
        public TextAreaData(string data)
        {
            this.data = data;
        }
        private string data { get; set; }

        public string Data
        {
            get
            {
                int lenght = IsLongText ? MaxLenght : data.Length;
                return IsLongText ? data.Substring(0, lenght) : data;
            }
        }

        public bool IsLongText
        {
            get
            {
                return data.Length > MaxLenght;
            }
        }

        public int MaxLenght = 100;
    }
}

