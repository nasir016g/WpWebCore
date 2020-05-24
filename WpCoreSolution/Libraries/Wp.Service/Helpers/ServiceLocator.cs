using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Service.Helpers
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }
}
