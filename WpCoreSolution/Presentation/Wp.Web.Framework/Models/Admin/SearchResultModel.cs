﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Web.Framework.Models
{
    public class SearchResultModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalRecords { get; set; }
    }
}
