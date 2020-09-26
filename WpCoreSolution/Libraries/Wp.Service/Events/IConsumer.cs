using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Services.Events
{
    public interface IConsumer<T>
    {
        void HandleEvent(T eventMessage);
    }
}
