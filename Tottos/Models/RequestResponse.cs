using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tottos.Models
{
    public class ResponceDto<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }

    public class RequestDto
    {
        public int draw { get; set; }
        public int length { get; set; }
        public int start { get; set; }
        public string search { get; set; }
    }
}