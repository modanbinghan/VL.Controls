using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.UcLibs
{
    public class CellProvider
    {
        Func<object> _ceateFunc;
        public CellProvider(Func<object> ceateFunc)
        {
            _ceateFunc = ceateFunc;
        }
        public object CreateCellObj()
        {
            if (_ceateFunc != null)
            {
                return _ceateFunc();
            }
            else
            {
                return null;
            }
        }
    }
}
