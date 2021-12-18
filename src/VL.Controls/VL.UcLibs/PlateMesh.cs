using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.UcLibs
{
    public struct PlateMesh
    {
        public PlateMesh(int rows,int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        private int _columns;
        public int Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }


        private int _rows;
        public int Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }
    }
}
