using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptOS.Core.Statistics
{
    public interface IStatisticsCruncher
    {
        string Name { get; }
        StatType Type { get; }

        void Crunch(ptOSContainer context, object obj);
    }
}
