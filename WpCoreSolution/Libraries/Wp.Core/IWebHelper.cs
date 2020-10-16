using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core
{
    public interface IWebHelper
    {
        string GetRawUrl(HttpRequest request);
    }
}
