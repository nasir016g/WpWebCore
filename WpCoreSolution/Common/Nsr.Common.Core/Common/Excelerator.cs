using System;
using System.Collections.Generic;
using System.Text;

namespace Nsr.Common.Core
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
