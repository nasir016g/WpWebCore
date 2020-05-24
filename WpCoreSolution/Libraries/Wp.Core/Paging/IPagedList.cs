using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core
{
    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalRecords { get; }
        int TotalPages { get; }
        
    }
}
