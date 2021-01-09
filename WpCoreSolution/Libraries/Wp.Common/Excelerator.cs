using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Common
{
    public interface IExcelerator
    {
        string GetData();
    }
    public class Excelerator
    {
        public string GetData()
        {
            return "The code buzz";
        }
    }
}
