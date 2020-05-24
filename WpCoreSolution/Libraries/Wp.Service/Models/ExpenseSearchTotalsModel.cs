using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Services.Models
{
    public class ExpenseSearchTotalsModel
    {
        public string TotalAmount { get; set; }
        public string SumNegative { get; set; }
        public string SumPositive { get; set; }
    }
}
