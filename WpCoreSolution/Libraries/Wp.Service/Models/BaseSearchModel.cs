using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Services.Models
{
    public class BaseSearchModel
    {
        public int PageSize { get; set; } = int.MaxValue;
        public int PageIndex { get; set; } = 0;
        public string SortField { get; set; } = "Id";
        public bool SortDescending { get; set; } = false;
    }
}
