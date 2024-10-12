using System;        
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace QuickCode.Turuncu.EmailManagerModule.Application.Models
{
    public class Response<T>
    {
        public string Message { get; init; } = "No Message";

        public int Code { get; set; }

        public T Value { get; set; } = default!;
    }
}
