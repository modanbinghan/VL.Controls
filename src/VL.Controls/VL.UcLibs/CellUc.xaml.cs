using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VL.UcLibs
{
    /// <summary>
    /// CellUc.xaml 的交互逻辑
    /// </summary>
    public partial class CellUc : UserControl
    {
        public CellUc(int row,int column)
        {
            InitializeComponent();
            _row = row;
            _column = column;
            _key = _row * 10000 + _column;
        }

        private int _row;
        public int Row
        {
            get { return _row; }
        }

        private int _column;
        public int Column
        {
            get { return _column; }
        }

        int _key;
        public int Key
        {
            get { return _key; }
        }
    }
}
