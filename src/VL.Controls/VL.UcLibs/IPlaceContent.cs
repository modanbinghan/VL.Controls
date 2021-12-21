using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.UcLibs
{
    public interface IPlaceContent
    {
        event EventHandler Deleted;
        int Row { get; }
        int Column { get; }
    }
}
